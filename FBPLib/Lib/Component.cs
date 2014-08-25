namespace FBPLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Text.RegularExpressions;
    using System.Reflection;

    /// <summary> All verbs must extend this class, defining its three abstract methods
    /// <code>openPorts<code>, <code>metadata<code>, and <code>execute</code>.
    /// *
    /// There will be an instance of this class for every node in the Network.
    /// *
    /// </summary>


    public abstract class Component
    {
        /* *
         * Copyright 2007,..., 2011, J. Paul Morrison.  At your option, you may copy,
         * distribute, or make derivative works under the terms of the Clarified Artistic License,
         * based on the Everything Development Company's Artistic License.  A document describing
         * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm.
         * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
         * */
        public string _name;

        private bool HasError()
        {
            return _status == States.Error;
        }

        public void InitBlock()
        {
            _inputPorts = new Dictionary<string, IInputPort>();
            _outputPorts = new Dictionary<string, OutputPort>();
            //_inputPortAttrs = new Dictionary<string, InPort>();
            //_outputPortAttrs = new Dictionary<string, OutPort>();
            _status = States.NotStarted;
            _stack = new Stack();
            _packetCount = 0;
            MustRun = false;
            SelfStarting = false;
            _timeout = null;
            //_event_dormant = new AutoResetEvent(false);
            _lockObject = new Object();
            //_compLog = new Logger();
            _mother = null;
            _autoStarting = false;

            System.Reflection.MemberInfo info = GetType();
            object[] attributes = info.GetCustomAttributes(true);
            foreach (object at in attributes)
            {
                if (at is MustRun)
                {
                    MustRun mr = at as MustRun;
                    MustRun = mr.value;
                }
                if (at is SelfStarting)
                {
                    SelfStarting sst = at as SelfStarting;
                    SelfStarting = sst.value;
                }
                if (at is OutPort)
                {
                    OutPort op = at as OutPort;
                    ProcOpt(op);

                }
                if (at is InPort)
                {
                    InPort ip = at as InPort;
                    ProcIpt(ip);
                }
                if (at is PriorityAttribute)
                {
                    PriorityAttribute p = at as PriorityAttribute;
                    Priority = p.value;
                }

            }
        }

        protected internal void ProcIpt(InPort ipt)
        {
            if (!(ipt.value.Equals("") ^ ipt.valueList == null))
            {
                FlowError.Complain(_name + ": @InPort must have value or valueList, but not both");
            }
            String s;
            if (!ipt.value.Equals(""))
            {
                s = ipt.value;
            }
            else
            {
                s = ipt.valueList[0];
            }
            if (ipt.fixedSize && !ipt.arrayPort)
            {
                FlowError.Complain(_name + "." + s + ": @InPort specified fixedSize but not arrayPort");
            }
            if (!ipt.value.Equals(""))
            {
                if (ipt.setDimension > 0 && !ipt.value.EndsWith("*"))
                {
                    FlowError.Complain(_name + "." + s
                        + ": @InPort specified setDimension but value string did not end with asterisk");
                }
                ProcIptx(ipt.value, ipt);
            }
            else
            {
                bool asterisk_found = false;
                foreach (String t in ipt.valueList)
                {
                    if (t.EndsWith("*"))
                    {
                        asterisk_found = true;
                    }
                    ProcIptx(t, ipt);
                }
                if (!asterisk_found && ipt.setDimension > 0)
                {
                    FlowError.Complain(_name + "." + s
                        + ": @InPort specified setDimension but valueList did not contain any strings ending with asterisks");
                }
            }
        }


        void ProcIptx(string s, InPort ipt)
        {
            int i = ipt.setDimension;
            String t = s;
            if (!s.EndsWith("*"))
            {
                i = 0;
            }
            else
            {
                t = s.Substring(0, s.Length - 1);
                if (i == 0)
                {
                    FlowError.Complain(_name + "." + s
                        + ": Asterisk specified on input port name, but setDimension was not specified");
                }
            }

            if (i == 0)
            {
                ProcIpty(t, ipt);
            }
            else
            {
                for (int j = 0; j < i; j++)
                {
                    ProcIpty(t + j, ipt);
                }
            }
        }
        protected internal void ProcIpty(string s, InPort ipt)
        {
            ConnArray ca = null;
            if (ipt.arrayPort)
            {
                ca = new ConnArray();
                _inputPorts.Add(s, ca);
                ca._fixedSize = ipt.fixedSize;
                ca._name = s;
                ca._optional = ipt.optional;
            }
            else
            {
                NullConnection nc = new NullConnection();
                nc._name = s;
                _inputPorts.Add(s, nc);
                nc._optional = ipt.optional;
            }
        }

        void ProcOpt(OutPort opt)
        {
            if (!(opt.value.Equals("") ^ opt.valueList == null))
            {
                FlowError.Complain(_name + ": @OutPort must have value or valueList, but not both");
            }
            String s;
            if (!opt.value.Equals(""))
            {
                s = opt.value;
            }
            else
            {
                s = opt.valueList[0];
            }
            if (opt.fixedSize && !opt.arrayPort)
            {
                FlowError.Complain(_name + "." + s + ": @OutPort specified fixedSize but not arrayPort");
            }
            if (!opt.value.Equals(""))
            {
                if (opt.setDimension > 0 && !opt.value.EndsWith("*"))
                {
                    FlowError.Complain(_name + "." + s
                        + ": @OutPort specified setDimension but value string did not end with asterisk");
                }
                ProcOptx(opt.value, opt);
            }
            else
            {
                bool asterisk_found = false;
                foreach (String t in opt.valueList)
                {
                    if (t.EndsWith("*"))
                    {
                        asterisk_found = true;
                    }
                    ProcOptx(t, opt);
                }
                if (!asterisk_found && opt.setDimension > 0)
                {
                    FlowError.Complain(_name + "." + s
                        + ": @OutPort specified setDimension but valueList did not contain any strings ending with asterisks");
                }
            }
        }

        void ProcOptx(String s, OutPort opt)
        {
            int i = opt.setDimension;
            String t = s;
            if (!s.EndsWith("*"))
            {
                i = 0;
            }
            else
            {
                t = s.Substring(0, s.Length - 1);
                if (i == 0)
                {
                    FlowError.Complain(_name + "." + s
                        + ": Asterisk specified on output port name, but setDimension was not specified");
                }
            }
            if (i == 0)
            {
                ProcOpty(t, opt);
            }
            else
            {
                for (int j = 0; j < i; j++)
                {
                    ProcOpty(t + j, opt);
                }
            }
        }
        protected internal void ProcOpty(string s, OutPort opt)
        {
            if (opt.arrayPort)
            {
                OutArray oa = new OutArray();
                _outputPorts.Add(s, oa);
                oa._fixedSize = opt.fixedSize;
                oa._optional = opt.optional;
            }
            else
            {
                OutputPort op = new NullOutputPort();
                op._optional = opt.optional;
                op.SetSender(this);
                _outputPorts.Add(s, op);
                op._name = s;
            }
        }

        public virtual string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }

        }
        internal virtual int PacketCount
        {
            get
            {
                return _packetCount;
            }

        }
        internal virtual States Status
        {
            get
            {
                return _status;
            }
            set { _status = value; }

        }
        internal virtual TimeoutHandler Timeout
        {
            get
            {
                return _timeout;
            }
            set { _timeout = value; }

        }
        internal virtual Type Type
        {
            get
            {
                return _type;
            }
            set { _type = value; }

        }

        internal virtual ThreadPriority Priority
        {
            get
            {
                return _priority;
            }
            set { _priority = value; }

        }


        protected internal System.Threading.Thread _thread;

        // All the input ports are stored here, keyed by name.
        internal Dictionary<string, IInputPort> _inputPorts;

        // All the output ports are stored here, keyed by name.
        internal Dictionary<string, OutputPort> _outputPorts;

        // Input port attributes
        //internal Dictionary<string, InPort> _inputPortAttrs;

        // Output port attributes
        //internal Dictionary<string, OutPort> _outputPortAttrs;

        /// <summary> This is a stack which is made available to each Component.
        /// Rather than declaring a special API, we make it <code>protected</code>
        /// so that the Component can use the regular java.util.Stack API.
        /// *
        /// </summary>

        protected Stack _stack;
        internal enum States
        {
            NotStarted, Active, Dormant, SuspRecv, SuspSend,
            Terminated, LongWait, Error
        }

        internal States _status;

        // This is the automatic input port named "*IN"
        internal IInputPort _autoInput;

        // This is the automatic output port named "*OUT"
        internal OutputPort _autoOutput;

        // This is a count of packets owned by this Component.
        // Whenever the Component deactivates, the count must be zero.
        internal int _packetCount;

        internal Type _type;

        // internal bool terminating;   no longer needed
        internal bool MustRun;
        internal bool SelfStarting;
        internal bool _autoStarting;

        // internal long packetNumber = 0; //debugging only
        //  internal Hashtable ownedPackets = null; //debugging only

        public Network _network;

        TimeoutHandler _timeout = null;
        //AutoResetEvent _event_dormant;

        //internal Logger _compLog;
        public Object _lockObject;

        internal Network _mother;


        internal ThreadPriority _priority = ThreadPriority.Normal;

        public Component()
        {
            InitBlock();

        }
        internal static Component NewComponent(Type type, string name, Network network)
        {

            ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
            Component cp = null;
            try
            {
                cp = (Component)ci.Invoke(null);
            }
            catch (TargetInvocationException e)
            {
                FlowError.Complain("Target Invocation Exception to " + type.FullName);
                return null; // unreachable
            }
            //cp._type = type;
            //cp._name = name;
            cp._mother = network;
            cp._type = type;
            cp._name = name;
            cp._network = network;
            return cp;
        }

        /// <summary>This method is called from other parts of the system
        /// to activate this Component if it needs to be.
        /// This will start its thread if needed, and if already
        /// started, will Pulse() it.
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'activate'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        internal virtual void Activate()
        {
            //  lock (this)
            //  {

            if (IsTerminated())
                return;

            if (_thread == null)
            {
                _thread = new Thread(new ThreadStart(ThreadMain));
                _thread.Name = Name;
            }
            if (!_thread.IsAlive)
            {
                _thread.Start();
                _thread.Priority = this.Priority;
            }

            else
            {
                //lock ((_inputPorts as ICollection).SyncRoot)
                //{
                try
                {
                    Monitor.Enter(_lockObject);
                    if (Status == States.Dormant)

                        //_event_dormant.Set();
                        //Monitor.Pulse((_inputPorts as ICollection).SyncRoot);
                        Monitor.Pulse(_lockObject);
                }
                finally
                {
                    Monitor.Exit(_lockObject);
                }

            }
        }
        /// <summary> This method creates a new Packet using a type and string, say for brackets
        /// </summary>
        /// <returns>javaflow.Packet
        /// </returns>
        /// <param name="s">java.lang.String
        /// 
        /// </param>
        public virtual Packet Create(Packet.Types newType, string s)
        {
            Interlocked.Increment(ref Network.creates);
            return new Packet(newType, s, this);
        }
        /// <summary> This method creates a Packet pointing at an Object
        /// </summary>
        /// <returns>javaflow.Packet
        /// </returns>
        /// <param name="o">java.lang.Object
        /// 
        /// </param>
        public virtual Packet Create(System.Object o)
        {
            Interlocked.Increment(ref Network.creates);
            return new Packet(o, this);
        }
        internal void CheckOwner(Packet p)
        {
            if (this != p._owner)
                FlowError.Complain("Packet not owned by current Component");
        }

        /**
         * Pop from stack. Return null if empty.
         **/
        Packet Pop()
        {
            Packet p;
            if (_stack.Count > 0)
                p = (Packet)_stack.Pop();
            else
                p = null;
            return p;
        }

        /**
         * Push onto stack.
         **/
        void Push(Packet p)
        {
            _stack.Push(p);
        }

        int StackSize()
        {
            return _stack.Count;
        }

        /* void TestIIPsClosed()
        {
            foreach (IInputPort i in _inputPorts.Values)
            {
                if (i is InitializationConnection)
                {
                    InitializationConnection ic = i as InitializationConnection;
                    if (!ic.IsClosed()) { FlowError.Complain("IIP " + ic.Name + " not closed"); }
                }
            }
        }
         */

        /// <summary> Verbs override this method with their program.
        /// This method is called from a private thread.
        /// </summary>
        /// <exception cref="">Throwable if there is any error or exception in the program;
        /// this is a kludge until we work out how to integrate exceptions properly.
        /// *
        /// </exception>

        public abstract void Execute();
        /// <summary> Return Network object
        /// </summary>
        /// <returns>Network
        /// 
        /// </returns>


        /*  Following JavaFBP, the following method has been retired in favour of metadata
        *
        * <summary> Obtain status of Component
        * </summary>
        * <returns>int
        * 
        * </returns>
        * <summary> Verbs override this method to provide information about their
        * ports to a <cite>Flowbox</cite>.  Although the result is
        * declared as an array of <code>Object</code>s, it must actually contain a
        * <code>string</code> describing the verb as a whole, followed by a
        * sequence of <i>descriptor</i>s with one per port,
        * where a <i>descriptor</i> is represented by four <code>Object</code>s
        * in a row.  These are: the port name (as a <code>string</code>);
        * the port type (one of <code>input</code>, <code>input[]</code>, <code>output</code>,
        * <code>output[]</code>, or <code>parameter</code>);
        * the Java type of objects passing through the port (either a <code>Class</code>
        * or <code>null</code>, which means the same as <code>Object.class</code>);
        * a human-readable description (a <code>string</code>)
        * suitable for use in the sentence <cite>"This port contains ..."</cite>.
        * *
        * </summary>

         public abstract System.Object[] Introspect();
        */
        /// <summary> This method returns <code> true </code> if the Component has terminated
        /// </summary>
        /// <returns>boolean
        /// 
        /// </returns>
        internal virtual bool IsTerminated()
        {
            return _status == States.Terminated;
        }
        /// <summary> Verbs call this method from their <code>openPorts</code> method to
        /// open an IInputPort, either a regular port or a parameter port.
        /// </summary>
        /// <param name="name">the name of the IInputPort
        /// </param>
        /// <returns>the IInputPort, which should be assigned to an
        /// instance variable of the verb
        /// *
        /// </returns>

        protected internal IInputPort OpenInput(string name)
        {
            if (name.StartsWith("*"))
                FlowError.Complain("Attempt to open * port: " + this + "." + name);

            if (!_inputPorts.ContainsKey(name))
                FlowError.Complain("Port not specifed in metadata: " + this + "." + name);
            Object o = _inputPorts[name];

            if (o is ConnArray)
                FlowError.Complain("Port is defined as input array in metadata: " + this._name + "." + name);

            return (IInputPort)o;
        }
        /// <summary> Verbs call this method from their <code>openPorts</code> method to
        /// open an array of inputPorts.  The size of the Array is determined
        /// by the Network to which this Component belongs, and thus the
        /// verb does not know it until <code>execute</code> time.
        /// </summary>
        /// <param name="the">name of the IInputPort array
        /// </param>
        /// <returns>the IInputPort[] object, which should be assigned to an instance variable.
        /// *
        /// </returns>

        // Array ports are stored individually in the inputPorts
        // and outputPorts variables with names like NAME[X], where
        // X is the index number.

        protected internal IInputPort[] OpenInputArray(string name)
        {
            return OpenInputArray(name, 0);
        }

        protected internal IInputPort[] OpenInputArray(string name, int arraySize)
        {
            if (name.StartsWith("*"))
                FlowError.Complain("Attempt to open * port: " + this + "." + name);

            ConnArray ca = null;
            if (!_inputPorts.ContainsKey(name))
                FlowError.Complain("Port not defined as input array in metadata: " + this._name + "." + name);
            Object o = _inputPorts[name];
            if (!(o is ConnArray))
                FlowError.Complain("Port not defined as input array in metadata: " + this._name + "." + name);
            ca = (ConnArray)o;
            if (!(ca._fixedSize ^ arraySize == 0))
            {
                FlowError.Complain("Array port fixedSize option in metadata doesn't match specified size: " + this._name
                    + "." + name);
            }
            IInputPort[] array = GetPortArray(_inputPorts, name);

            if (arraySize > 0 && array.Length > arraySize)
            {
                FlowError.Complain("Number of elements specified for array port less than actual number used: "
                    + this._name + "." + name);
            }

            if (arraySize > 0 && array.Length < arraySize)
            {
                Array.Resize(ref array, arraySize);
            }

            for (int i = 0; i < array.Length; i++)
                if (array[i] == null)
                {
                    NullConnection nc = new NullConnection();
                    nc.Name = Name + "." + name + "[" + i + "]";
                    array[i] = nc;
                }
            _inputPorts.Remove(name);
            return array;

        }


        /// <summary> Verbs call this method from their <code>openPorts</code> method
        /// to open an output port.
        /// </summary>
        /// <param name="name">the name of the OutputPort
        /// </param>
        /// <returns>the OutputPort, which should be assigned to an instance variable
        /// *
        /// </returns>

        protected internal OutputPort OpenOutput(string name)
        {
            if (name.StartsWith("*"))
                FlowError.Complain("Attempt to open * port: " + this + "." + name);

            if (!_outputPorts.ContainsKey(name))
                FlowError.Complain("Port not specified in metadata: " + this + "." + name);
            Object o = _outputPorts[name];

            if (o is OutArray)
                FlowError.Complain("Port is defined as output array in metadata: " + this._name + "." + name);
            return (OutputPort)o;
        }


        /// <summary> Verbs call this method from their <code>openPorts</code> method to
        /// open an array of OutputPorts.  The size of the Array is determined
        /// by the Network to which this Component belongs, and thus the
        /// verb does not know it until <code>execute</code> time.
        /// </summary>
        /// <param name="the">name of the OutputPort array
        /// </param>
        /// <returns>the OutputPort[] object, which should be assigned to an instance variable.
        /// *
        /// </returns>

        // See openInputArray.
        protected internal OutputPort[] OpenOutputArray(string name)
        {
            return OpenOutputArray(name, 0);
        }
        protected internal OutputPort[] OpenOutputArray(string name, int arraySize)
        {
            if (name.StartsWith("*"))
                FlowError.Complain("Attempt to open * port: " + this + "." + name);

            OutArray oa = null;
            if (!_outputPorts.ContainsKey(name))
                FlowError.Complain("Port not defined as output array in metadata: " + this._name + "." + name);
            Object o = _outputPorts[name];
            if (!(o is OutArray))
                FlowError.Complain("Port not defined as output array in metadata: " + this._name + "." + name);
            oa = (OutArray)o;

            if (!(oa._fixedSize ^ arraySize == 0))
            {
                FlowError.Complain("Array port fixedSize option in metadata doesn't match specified size: " + this._name
                    + "." + name);
            }
            OutputPort[] array = GetPortArray(_outputPorts, name);

            if (array == null && !oa._optional)
            {
                FlowError.Complain("No elements defined in mandatory output array port: " + this._name + "." + name);
            }

            if (array != null)
            {

                if (arraySize > 0 && array.Length > arraySize)
                {
                    FlowError.Complain("Number of elements specified for array port less than actual number used: "
                        + this._name + "." + name);
                }

                if (arraySize > 0 && array.Length < arraySize)
                {
                    Array.Resize(ref array, arraySize);
                }

                for (int i = 0; i < array.Length; i++)
                    if (array[i] == null)
                    {
                        if (!oa._optional && arraySize > 0)
                        {
                            FlowError.Complain("Mandatory output array port has missing elements: " + this._name + "." + name);
                        }
                        array[i] = new NullOutputPort();
                        array[i]._sender = this;
                        array[i].Name = Name + "." + name + "[" + i + "]";

                    }
            }
            _outputPorts.Remove(name);
            return array;
        }

        // Get array of ports in the form name[]
        T[] GetPortArray<T>(Dictionary<string, T> ports, string name)
        {
            T[] ret = null;
            Regex re = new Regex(@"^(\w+)(\[(\d+)\])?$");

            foreach (KeyValuePair<string, T> kvp in ports)
            {
                Match m = re.Match(kvp.Key);
                if (!m.Success)
                    FlowError.Complain("Invalid port name :" + kvp.Key);
                if (!(kvp.Value is T))    // ignore other types than IInputPort or OutputPort (depending on T)
                    continue;
                string s = m.Groups[1].Value;
                if (!s.Equals(name)) continue;
                int subs = 0;
                if (m.Groups[2].Value.Equals(""))
                    continue;
                subs = Convert.ToInt32(m.Groups[3].Value);

                if (subs < 0 || subs >= 1000)
                    FlowError.Complain("bad subscript " + name);

                if (ret == null)
                    ret = new T[0];
                if (subs >= ret.Length)
                    Array.Resize(ref ret, subs + 1);
                ret[subs] = kvp.Value;
            }
            return ret;
        }

        /// <summary>Drop Packet and clear owner reference.
        /// Note: Java will not reclaim the space until after all references
        /// to this packet have been cleared - ideally, one should not use this
        /// knowledge, but bright people probably will!
        /// </summary>
        public void Drop(Packet p)
        {
            CheckOwner(p);
            Interlocked.Increment(ref Network.drops);
            p.ClearOwner();
            p.Dispose();

        }

        /// <summary> Verbs override this method to open their ports.
        /// This method is called from the Network's main thread,
        /// and should only call <code>openInput</code>, <code>openInputArray</code>,
        /// <code>openOutput</code>, <code>openOutputArray<code>, <code>IInputPort.setType</code>,
        /// and <code>OutputPort.setType</code>.
        /// *
        /// </summary>

        public abstract void OpenPorts();


        /// <summary>
        /// Nested class to get state of all ports as an object, thread safe.
        /// </summary>
        /// 
        class InputStates
        {
            // true if all connected input ports are closed and empty
            internal bool allDrained = true;

            // true if any connected input port has data
            internal bool hasData = false;


            // Get state of all ports

            internal InputStates(Dictionary<string, IInputPort> inports, Component comp)
            {
                //lock ((inports as ICollection).SyncRoot)
                //{
                try
                {
                    Monitor.Enter(comp._lockObject);
                    while (true)
                    {
                        allDrained = true;
                        hasData = false;
                        foreach (IInputPort inp in inports.Values)
                            if (inp is Connection)
                            {
                                Connection c = inp as Connection;
                                // lock (c)
                                // {
                                //allDrained &= c.IsDrained();
                                allDrained &= c._buffer._usedSlots == 0 && c._senderCount == 0;
                                // hasData |= !c.IsEmpty();
                                hasData |= c._buffer._usedSlots > 0;
                                // }
                            }
                        if (allDrained || hasData)
                            break;
                        comp._status = States.Dormant;

                        comp._mother.Trace("{0}: Dormant", comp.Name);
                        //Monitor.Wait((inports as ICollection).SyncRoot);
                        //comp._event_dormant.Reset();
                        //comp._event_dormant.WaitOne();
                        Monitor.Wait(comp._lockObject);
                        comp.Status = States.Active;
                        comp._mother.Trace("{0}: Active", comp.Name);
                    }
                }
                finally
                {
                    Monitor.Exit(comp._lockObject);
                }
                //if (_network._deadlock)
                //{
                //    comp._thread.Interrupt();
                //     return;
                // }
            }
        }

     public bool CheckPorts() {
            bool res = true;
         
	        foreach (KeyValuePair<String, IInputPort> kvp in _inputPorts) {
	            if (kvp.Value is NullConnection) {
                    NullConnection nc = (NullConnection)kvp.Value;
                    if (nc._optional)
                    {
                        continue;
                    }
	                 Console.WriteLine("Input port specified in metadata, but not connected: " +  Name + "."  + 
	               kvp.Value.Name);
	            res = false;
	            }
	        }
        
            foreach (KeyValuePair<String, OutputPort> kvp in _outputPorts)  {
                if (kvp.Value is NullOutputPort) {
                   NullOutputPort nop = (NullOutputPort) kvp.Value;
                   if (nop._optional) {
                              continue;
                   }

                 Console.WriteLine("Output port specified in metadata, but not connected: " +  Name + "."  + 
                     kvp.Value.Name);
                 res = false;
               }
            }
            return res;
        }

        // Use SetPriority in Execute, not OpenPorts!
        //public void SetPriority(ThreadPriority p) { _thread.Priority = p; }

        public void LongWaitStart(double dur)
        {
            Timeout = new TimeoutHandler(dur, this);
        }

        public void LongWaitEnd()
        {
            Timeout.Dispose();
        }

        //UPGRADE_TODO: The equivalent of method java.lang.Runnable.run is not an override method. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca5065"'
        // internal void Run()
        // implement mainline for thread

        // runs and keeps running as long as there are input ports to read
        void ThreadMain()
        {
            try
            {
                if (IsTerminated() || HasError())
                {
                    try
                    {
                        Monitor.Exit(_lockObject);
                    }
                    catch (SynchronizationLockException e)
                    {
                        // do nothing - this is OK!
                    }
                    return;
                }
                _status = States.Active;

                _mother.Trace("{0}: Started", Name);

                if (!_inputPorts.ContainsKey("*IN"))
                    _autoInput = null;
                else
                    _autoInput = (IInputPort)(_inputPorts["*IN"]);
                if (!_outputPorts.ContainsKey("*OUT"))
                    _autoOutput = null;
                else
                    _autoOutput = (OutputPort)(_outputPorts["*OUT"]);

                if (_autoInput != null)
                {
                    Packet p = _autoInput.Receive();
                    if (p != null)
                        Drop(p);
                    _autoInput.Close();
                }

                InputStates ist = null;
                if (SelfStarting)
                    _autoStarting = true;
                else
                {
                    try
                    {
                        ist = new InputStates(_inputPorts, this);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        if (IsTerminated() || HasError())
                        {
                            // if we are in the TERMINATED or ERROR state we terminated intentionally
                            return;
                        }
                        // otherwise there was an error
                        throw ex;

                    }

                }

                while (_autoStarting || !ist.allDrained || _autoInput != null || ist.allDrained && MustRun || StackSize() > 0)
                {
                    _autoInput = null;
                    if (_network._deadlock || IsTerminated())
                    {
                        break;
                    }
                    _packetCount = 0;

                   
                    foreach (IInputPort port in _inputPorts.Values)
                    {
                        if (port is InitializationConnection)
                        {
                            InitializationConnection icx = port as InitializationConnection;
                            icx.Reopen();
                        }
                    }

                    _mother.Trace("{0}: Activated", Name);
                    try
                    {
                        Execute(); // do one activation!
                    }
                    catch (ComponentException e)
                    {
                        _mother.Trace("Component Exception: " + Name + " - " + e.Message);
                        if (e.Message.StartsWith("*"))
                        {
                            string s = e.Message.Substring(1);
                            FlowError.Complain("Component Exception: " + Name + " - " + s);
                        }
                        else
                            Console.Out.WriteLine("! Component Exception: " + Name + " - " + e.Message);
                    }

                    _mother.Trace("{0}: Deactivated", Name);

                    if (_packetCount != 0)
                    {
                        _mother.Trace(Name + " deactivated holding " + _packetCount + " packets");

                        FlowError.Complain(_packetCount + " packets not disposed of during Component activation of " + Name);
                    }
                    foreach (IInputPort port in _inputPorts.Values)
                    {
                        if (port is InitializationConnection)
                        {
                            InitializationConnection icx = port as InitializationConnection;
                            if (!icx.IsClosed())
                                FlowError.Complain("Component deactivated with IIP port not closed: " + icx.Name);
                        }
                    }
                    MustRun = false;
                    SelfStarting = false;
                    if (_autoStarting) break;
                    // lock ((_inputPorts as ICollection).SyncRoot)
                    //{
                    try
                    {
                        ist = new InputStates(_inputPorts, this);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        if (IsTerminated() || HasError())
                        {
                            // if we are in the TERMINATED or ERROR state we terminated intentionally
                            return;
                        }
                        // otherwise there was an error
                        throw ex;

                    }
                    if (ist.allDrained)
                        break;
                    //if (_network._deadlock)
                    //{
                    //    break;
                    // }

                } //  while (!ist.allDrained);


                //_compLog.Trace("{0}: Terminating", Name);
                //}
                // catch (System.Exception t)
                // {
                //Console.Out.WriteLine("*** Exception detected in " + Name);
                //      System.Diagnostics.Trace.Fail("*** Exception detected in " + Name + ": " + t.Message);
                // }


                //_compLog.Trace("{0}: Terminated", Name);
                if (_autoOutput != null)
                {
                    //Packet p = Create("");
                    //_autoOutput.Send(p);
                    _autoOutput.Close();

                }
                _status = States.Terminated;

                if (_stack.Count > 0)
                    FlowError.Complain("Stack not empty at component termination: " + Name);

                foreach (IInputPort port in _inputPorts.Values)
                {
                    if (port is Connection)
                    {
                        Connection cx = port as Connection;
                        if (cx.Count() > 0)
                            Console.Out.WriteLine("{0}: Component terminated with {1} packets in input connection", cx.Name, cx.Count());
                        while (cx.Count() > 0)
                        {
                            Packet p = cx._buffer.Take();
                            Console.Out.WriteLine(p);
                        }


                    }
                    if (port is InitializationConnection)
                    {
                        InitializationConnection iip = port as InitializationConnection;
                        if (!(iip.IsClosed()))
                            FlowError.Complain("Component terminated with input port not closed: " + iip.Name);
                    }
                }

                foreach (OutputPort port in _outputPorts.Values)
                {
                    port.Close();
                }

                //_status = States.Terminated; //will not be set if never activated
                //_network.NotifyTerminated();
                _mother.NotifyTerminated(this);

            }
            catch (Exception e)
            {
                // don't tell the mother if we are already in the ERROR or TERMINATE state
                // because then the mother told us to terminate
                if (!HasError() && !IsTerminated())
                {
                    // an error occurred in this component
                    _status = States.Error;
                    // tell the mother
                    _mother.SignalError(e);
                }
            }
        }
        /**
* Terminates the component.
* 
* @param newStatus the new status of the component (mostly TERMINATED or ERROR)
*/
        internal virtual void Terminate(States newStatus)
        {
            _status = newStatus;
            if (_thread != null)
                _thread.Interrupt();
        }
    }
}




