namespace FBPLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.IO;
    using System.Threading;
    using Spring.Threading.Helpers;   // CountDownLatch  
    //  using System.Text.RegularExpressions;

    using FBPLib.Properties;


    /// <summary> The abstract class which all flow Networks extend directly or
    /// indirectly.  A specific flow Network must override the <code>define()</code>
    /// method, which is written using the <i>mini-language</i>
    /// (actually just highly restricted Java invoking the <code>protected</code>
    /// methods of this class).  The mini-language specifies what
    /// threads are to be created using which components, and what
    /// connections are established between the ports of those components.
    /// Here is a sample: 
    /// <pre>
    /// Connect(Component("Generate", typeof(Generate)),
    ///		Port("OUT"),
    ///        Component("Write", typeof(WriteText)),
    ///		Port("IN"));
    ///    
    ///    
    ///	Initialize("100",
    ///		Component("Generate"),
    ///		Port("COUNT"));
    ///
    ///    Stream st = Console.OpenStandardOutput();
    ///	Initialize(st,
    ///		Component("Write"),
    ///        Port("DESTINATION"));
    ///    Initialize("", 
    ///        Component("Write"),
    ///        Port("FLUSH"));
    /// </pre>
    /// The effect of the above is to create two threads named <code>Read</code>
    /// (an instance of <code>ReadText</code>) and <code>Write</code> (an instance of
    /// <code>WriteText</code>, connect the <code>OUT</code> port of the former to
    /// the <code>IN</code> port of the latter, and initialize the <code>SOURCE</code>
    /// and <code>DESTINATION</code> parameter ports with suitable C# objects
    /// for reading the standard input and writing the standard output.
    /// *
    /// </summary>

    public abstract class Network : Component
    {
        /* *
           * Copyright 2007, ..., 2011, J. Paul Morrison.  At your option, you may copy, 
           * distribute, or make derivative works under the terms of the Clarified Artistic License, 
           * based on the Everything Development Company's Artistic License.  A document describing 
           * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
           * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
           * */


        protected internal static int DEBUGSIZE = 1;

        protected internal static int PRODUCTIONSIZE = 10;

        internal static int _defaultCapacity = DEBUGSIZE;       // change this when you go to production
        //internal static int _defaultCapacity = PRODUCTIONSIZE; // use this one for production

        //UPGRADE_NOTE: The initialization of  'components' was moved to method 'InitBlock'. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1005"'
        internal Dictionary<string, Component> _components;

        internal bool _active = true;   // for deadlock detection   

        internal Dictionary<Component, TimeoutHandler> _timeouts = new Dictionary<Component, TimeoutHandler>();

        Thread _mainthread = null;

        public CountDownLatch _cdl;

        //public static Network _topNetwork;

        //static internal string _netname;

        public object[] parms;

        string _strPath;

        internal volatile bool _deadlock;

        private Exception _error;
        bool _abort = false;

        public bool _deadlockTest = true; // false if SubNet
        List<String> _msgs = null;
        public static bool _tracing;
        public static String _tracePath;
        private StreamWriter _traceWriter;
        public bool _useConsole = false;
        public static bool _forceConsole = false;
        public static List<StreamWriter> _traceFileList = null;
        public static int sends, receives, creates, drops, dropOlds;

        public new void InitBlock()
        {
            _components = new Dictionary<string, Component>();
            //_network = this;

            // Get the directory our exe is running from.
            _strPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            _strPath = _strPath.Substring(0, _strPath.LastIndexOf('\\'));
            Settings s = Settings.Default;
            _tracing = s.Tracing;  // get value from Properties for FBPLib
            _tracePath = @"C:\Temp\";
            receives = 0;
            sends = 0;
            creates = 0;
            drops = 0;
            dropOlds = 0;

            if (!(this is SubNet))  // i.e. Network only!
            {
                int i = Name.LastIndexOf(".");
                if (i != -1)
                    Name = Name.Substring(i + 1);
                _traceFileList = new List<StreamWriter>();
            }
        }
        // return object to use for synchronising state changes
        internal object StateSynchRoot
        {
            get { return (_components as ICollection).SyncRoot; }
        }

        /// <summary>Drive define method of Network
        /// </summary>

        internal virtual void CallDefine()
        {
            // don't turn every exception into a FlowError
            //try
            //{
            Define();
            //}
            //catch (System.Exception t)
            //{
            //     FlowError.Complain(t.Message);
            // }
        }

        /// <summary>Returns a Component class object, given the Component name in the Network
        /// </summary>		
        protected internal Component Component(string name)
        {
            if (!_components.ContainsKey(name))
                FlowError.Complain("Reference to unknown Component " + name);
            Component c = (Component)(_components[name]);

            c.Name = name;
            return c;
        }
        /// <summary>Stores the Component class object with its Network name in the Hashtable called
        /// 'components'
        /// </summary>		
        protected internal Component Component(string name, System.Type type)
        {
            if (_components.ContainsKey(name))
                FlowError.Complain("Attempt to redefine Component " + name);
            Component c = null;
            //UPGRADE_NOTE: Exception 'java.lang.InstantiationException' was converted to ' ' which has different behavior. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1100"'
            Type foundType = null;
            Assembly engine = System.Reflection.Assembly.GetCallingAssembly();
            foundType = FindType(engine, type);

            if (foundType == null)
            {
                // Search for all DLLs in the folder 
                foreach (string strFile in Directory.GetFiles(_strPath, "*.dll"))
                {
                    // Open the class library.
                    engine = Assembly.LoadFrom(strFile);

                    foundType = FindType(engine, type);
                    if (foundType != null)
                        break;
                }
            }
            if (foundType == null)
                FlowError.Complain("Component not found: " + type.FullName);

            try
            {
                //ConstructorInfo cnst = foundType.GetConstructor(Type.EmptyTypes);
                //c = (Component)(cnst.Invoke(null));
                c = NewComponent(foundType, name, this);
            }
            catch (System.UnauthorizedAccessException e)
            {
                FlowError.Complain("Illegal access to Component " + type.FullName);
                return null; // unreachable
            }
            catch (System.Exception e)
            {
                FlowError.Complain(e.ToString());
                return null; // unreachable
            }

            c.Type = foundType;
            c.Name = name;
            //c._network = this;
            //c._mother = this;
            _components.Add(name, c);
            c._network = this;
            return c;
        }

        internal Type FindType(Assembly engine, Type type)
        {
            Type[] types = engine.GetTypes();
            foreach (Type t in types)
            {
                if ((t.FullName == type.FullName) & (t.IsClass))
                {
                    return t;
                }
            }
            return null;
        }

        /* Connects */

        /// <summary>Connect an output port of one Component to an input port
        /// of another
        /// </summary>

        protected internal Connection Connect(Component sender, Port outP, Component receiver, Port inP, int size, bool IPCount)
        {
            int cap = size;
            if (size == 0)
                cap = _defaultCapacity;

            //string outName = outPort.displayName;
            //string inName = inPort.displayName;
            
            if (outP._displayName.Equals("*"))
            {
                outP._name = "*OUT";
                outP._displayName = "*OUT";
            }
            if (inP._displayName.Equals("*"))
            {
                inP._name = "*IN";
                inP._displayName = "*IN";
            }

            OutputPort op = null;
            if (!outP._displayName.Substring(0, 1).Equals("*"))
            {
                op = sender._outputPorts[outP._name]; // try to find output port with port name - no index
                if (op == null)
                {
                    FlowError.Complain("Output port not defined in metadata: " + sender._name + "." + outP._displayName);
                }

                if (op is OutArray && outP._index == -1)
                {
                    outP._index = 0;
                    outP._displayName = outP._name + "[" + outP._index + "]";
                }



                if (outP._index > -1 && !(op is OutArray))
                {
                    FlowError.Complain("Output port not defined as array in metadata: " + sender._name + "." + outP._displayName);
                }

                if (!(op is NullOutputPort) && !(op is OutArray) && op._cnxt != null)
                {
                    FlowError.Complain("Multiple connections to output port:" + sender._name + ' ' + outP._displayName);
                }
            }

            op = new OutputPort();
            op.SetSender(sender);
            op._name = outP._displayName;
            op._connected = true;
            op._fullName = sender._name + "." + outP._displayName;
            op._traceNetwork = sender._mother;
                        
            sender._outputPorts.Remove(op._name);
            sender._outputPorts.Add(op._name, op);

            /* start processing input port */

            IInputPort ip = null;
            if (!inP._displayName.Substring(0, 1).Equals("*"))
            {
                ip = receiver._inputPorts[inP._name];
                if (ip == null)
                {
                    FlowError.Complain("Input port not defined in metadata: " + receiver._name + "." + inP._displayName);
                }

                if (ip is ConnArray && inP._index == -1)
                {
                    inP._index = 0;
                    inP._displayName = inP._name + "[" + inP._index + "]";
                }

                if (inP._index > -1 && !(ip is ConnArray))
                {
                    FlowError.Complain("Input port not defined as array in metadata: " + receiver._name + "." + inP._displayName);
                }
            }
            Connection c;
            if (ip is Connection)
            {
                if (size != 0 && size != cap)
                {
                    FlowError.Complain("Connection capacity does not agree with previous specification\n " + receiver._name
                        + "." + inP._displayName);
                }
                c = (Connection)ip;
            }
            else
            {
                if (ip is InitializationConnection)
                {
                    FlowError.Complain("Mixed connection to input port: " + receiver._name + "." + inP._displayName);
                }
                c = new Connection(cap);
                c.SetReceiver(receiver);
                c._name = inP._displayName;
                c._IPCount = IPCount;
                c._fullName = receiver._name + "." + c._name;
                receiver._inputPorts.Remove(c._name);
                receiver._inputPorts.Add(c._name, c);
            }

            c.BumpSenderCount();
            op._cnxt = c;
            c._receiver = receiver;
            c._fullName = receiver.Name + "." + inP._displayName;
            return c;
        }

        protected internal Connection Connect(Component sender, Port outP, string receiver, int size, bool IPCount)
        {
            string[] parts = CPSplit(receiver);
            return Connect(sender, outP, Component(parts[0]), Port(parts[1]), size, IPCount);
        }

        protected internal Connection Connect(string sender, Component receiver, Port inP, int size, bool IPCount)
        {
            string[] parts = CPSplit(sender);
            return Connect(Component(parts[0]), Port(parts[1]), receiver, inP, size, IPCount);
        }

        protected internal Connection Connect(string sender, string receiver, int size, bool IPCount)
        {
            string[] sParts, rParts;
            sParts = CPSplit(sender);
            rParts = CPSplit(receiver);

            return Connect(Component(sParts[0]), Port(sParts[1]),
               Component(rParts[0]), Port(rParts[1]), size, IPCount);
        }

        //---//

        protected internal Connection Connect(Component sender, Port outP, Component receiver, Port inP, int size)
        {
            return Connect(sender, outP, receiver, inP, size, false);
        }

        protected internal Connection Connect(Component sender, Port outP, string receiver, int size)
        {
            string[] parts = CPSplit(receiver);
            return Connect(sender, outP, Component(parts[0]), Port(parts[1]), size, false);
        }

        protected internal Connection Connect(string sender, Component receiver, Port inP, int size)
        {
            string[] parts = CPSplit(sender);
            return Connect(Component(parts[0]), Port(parts[1]), receiver, inP, size, false);
        }

        protected internal Connection Connect(string sender, string receiver, int size)
        {
            string[] sParts, rParts;
            sParts = CPSplit(sender);
            rParts = CPSplit(receiver);

            return Connect(Component(sParts[0]), Port(sParts[1]),
               Component(rParts[0]), Port(rParts[1]), size, false);
        }

        //---//
        protected internal Connection Connect(Component sender, Port outP, Component receiver, Port inP, bool IPCount)
        {
            return Connect(sender, outP, receiver, inP, 0, IPCount);
        }

        protected internal Connection Connect(Component sender, Port outP, string receiver, bool IPCount)
        {
            string[] parts = CPSplit(receiver);
            return Connect(sender, outP, Component(parts[0]), Port(parts[1]), 0, IPCount);
        }

        protected internal Connection Connect(string sender, Component receiver, Port inP, bool IPCount)
        {
            string[] parts = CPSplit(sender);
            return Connect(Component(parts[0]), Port(parts[1]), receiver, inP, 0, IPCount);
        }

        protected internal Connection Connect(string sender, string receiver, bool IPCount)
        {
            string[] sParts, rParts;
            sParts = CPSplit(sender);
            rParts = CPSplit(receiver);

            return Connect(Component(sParts[0]), Port(sParts[1]),
               Component(rParts[0]), Port(rParts[1]), 0, IPCount);
        }

        //---//
        protected internal Connection Connect(Component sender, Port outP, Component receiver, Port inP)
        {
            return Connect(sender, outP, receiver, inP, 0, false);
        }

        protected internal Connection Connect(Component sender, Port outP, string receiver)
        {
            string[] parts = CPSplit(receiver);
            return Connect(sender, outP, Component(parts[0]), Port(parts[1]), 0, false);
        }

        protected internal Connection Connect(string sender, Component receiver, Port inP)
        {
            string[] parts = CPSplit(sender);
            return Connect(Component(parts[0]), Port(parts[1]), receiver, inP, 0, false);
        }
        protected internal Connection Connect(string sender, string receiver)
        {
            string[] parts1 = CPSplit(sender);
            string[] parts2 = CPSplit(receiver);
            return Connect(Component(parts1[0]), Port(parts1[1]), Component(parts2[0]), Port(parts2[1]), 0, false);
        }


        //---//
        protected internal Connection Connect(Component sender, Port outP, Component receiver, Port inP, bool IPCount, int arraySize)
        {
            return Connect(sender, outP, receiver, inP, arraySize, IPCount);
        }

        protected internal Connection Connect(Component sender, Port outP, string receiver, bool IPCount, int arraySize)
        {
            string[] parts = CPSplit(receiver);
            return Connect(sender, outP, Component(parts[0]), Port(parts[1]), arraySize, IPCount);
        }

        protected internal Connection Connect(string sender, Component receiver, Port inP, bool IPCount, int arraySize)
        {
            string[] parts = CPSplit(sender);
            return Connect(Component(parts[0]), Port(parts[1]), receiver, inP, arraySize, IPCount);
        }

        protected internal Connection Connect(string sender, string receiver, bool IPCount, int arraySize)
        {
            string[] sParts, rParts;
            sParts = CPSplit(sender);
            rParts = CPSplit(receiver);

            return Connect(Component(sParts[0]), Port(sParts[1]),
               Component(rParts[0]), Port(rParts[1]), arraySize, IPCount);
        }

        //---//
        // splits a string into component part and port part

        string[] CPSplit(string s)
        {
            int i = s.IndexOf(".");
            if (i < 0)
            {
                FlowError.Complain("Invalid receiver string: " + s);
            }
            string[] p = { s.Substring(0, i), s.Substring(i + 1) };
            return p;
        }

        public abstract void Define();

        /// <summary>Execute method used by Network being used as if it were a Component
        /// </summary>
        public override void Execute()
        {
            // overridden by specific networks
        }


        /// <summary>Execute Network as a whole
        /// </summary>
        [MTAThreadAttribute]   // this the default, so not really necessary
        public void Go()
        {
            Type t = this.GetType();
            Name = t.FullName;

            _network = this;

            DateTime now = DateTime.Now;

            InitBlock();
            _mainthread = new Thread(delegate()
            {
                try
                {
                    CallDefine();
                    bool res = true;
                    foreach (Component comp in _components.Values) {
                        res &= comp.CheckPorts();
                    }
                    if (!res)
    	                  FlowError.Complain("One or more mandatory connections have been left unconnected: " + Name);

                    _cdl = new CountDownLatch(_components.Count);


                    Trace(Name + ": run started");
                    _active = true;

                    Initiate();

                    WaitForAll();
                }
                catch (FlowError e)
                {
                    string s = "Flow Error :" + e;

                    Console.Out.WriteLine("Network: " + s);
                    Console.Out.Flush();
                    // rethrow the exception for external error handling
                    // in case of a deadlock: deadlock is the cause
                    throw e;
                }

                if (_error != null)
                {
                    // throw the exception which caused the network to stop
                    throw _error;
                }

                TimeSpan duration = DateTime.Now - now;

                Console.Out.WriteLine("{0} - run time: {1}", Name, duration);
                
                Console.Out.WriteLine("Counts: C: {0}, D: {1}, S: {2}, R (non-null): {3}, DO: {4}", creates, drops, sends, receives, dropOlds);
                CloseTraceFiles();

            });

            _mainthread.Start();
        }

        
        protected internal void Initialize(object param, string receiver)
        {
            string r1, r2;
            int i;
            if ((i = receiver.IndexOf('.')) == -1)
                FlowError.Complain("invalid receiver name" + receiver);
            r1 = receiver.Substring(0, i);  // start, length
            r2 = receiver.Substring(i + 1);  // start

            Initialize(param, Component(r1), Port(r2));
        }
        /// <summary>Build InitializationConnection object
        /// </summary>

        /// <summary>Build InitializationConnection object
        /// </summary>

        protected internal void Initialize(System.Object content, Component receiver, Port inP)
        {
            IInputPort ip = receiver._inputPorts[inP._displayName];
            if (ip == null)
            {
                FlowError.Complain("Input port not defined in metadata: " + receiver._name + "." + inP._displayName);
            }
            if (ip is Connection || ip is ConnArray)
            {
                FlowError.Complain("IIP port cannot be shared: " + receiver._name + "." + inP._displayName);
            }

            if (ip is InitializationConnection)
            {
                FlowError.Complain("IIP port already used: " + receiver._name + "." + inP._displayName);
            }

            if (ip is ConnArray && inP._index == -1)
            {
                inP._index = 0;
                inP._displayName = inP._name + "[" + inP._index + "]";
            }

            if (inP._index > -1 && !(ip is ConnArray))
            {
                FlowError.Complain("Input port not defined as array in metadata: " + receiver._name + "." + inP._displayName);
            }

            InitializationConnection ic = new InitializationConnection(content, receiver);
            ic._name = receiver._name + "." + inP._displayName;
            //ic.network = this;

            receiver._inputPorts.Remove(inP._displayName);
            receiver._inputPorts.Add(inP._displayName, ic);

        }
        /// <summary>Go through components activating any that can be activated
        /// </summary>
        internal virtual void Initiate()
        {

            foreach (Component c in _components.Values)
            {
                c.OpenPorts();
            }
            ArrayList selfStarters = new ArrayList();
            foreach (Component c in _components.Values)
            {
                c._autoStarting = true;
                if (!SelfStarting)
                {

                    foreach (IInputPort port in c._inputPorts.Values)
                    {
                        if (port is Connection)
                        {
                            //cnxt.CheckTypes();                        
                            c._autoStarting = false;
                            break;
                        }
                    }
                }

                if (c._autoStarting)
                {
                    selfStarters.Add(c);
                }
            }

            foreach (Component c in selfStarters)
            {
                c.Activate();
            }
        }



        /// <summary>method to open ports for subnet
        /// </summary>
        public override void OpenPorts()
        {
            /*
            _inport = OpenInput("IN");
            _outport = OpenOutput("OUT");
            substreamSensitive = OpenInput("SUBSTREAM-SENSITIVE");
             * */
        }


        /// <summary>method to register a port name  
        /// </summary>

        public Port Port(string name)
        {
            if (!name.Equals("*") && !name.Equals("*SUBEND") && -1 != name.IndexOf('*'))
            {
                FlowError.Complain("Stray * in port name " + name);
            }
            Port p = new Port(name, -1);
            //p.displayName = String.Format("{0}", name);
            return p;
        }
        /// <summary>method to register a port (with index) 
        /// </summary>

        protected internal Port Port(string name, int index)
        {
            if (!name.Equals("*") && -1 != name.IndexOf('*'))
            {
                FlowError.Complain("Stray * in port name " + name);
            }
            Port p = new Port(name, index);
            //p.displayName = String.Format("{0}[{1}]", name, index);
            return p;
        }


        /// <summary>Test if Network as a whole has terminated
        /// </summary>

        internal virtual void WaitForAll()
        {
            bool possibleDeadlock = false;
            //bool deadlock = false;

            int freq = 500;   // check every .5 second
            Settings s = Settings.Default;
            while (true)
            {
                if (_cdl.Await(new TimeSpan(0, 0, 0, 0, freq)))   // 500 msecs 
                    break;  // if CountDownLatch finished, exit
                // if an error occurred, skip deadlock testing
                if (_error != null)
                {
                    break;
                }

                // if the network was aborted, skip deadlock testing 
                if (_abort)
                {
                    break;
                }
                if (!_deadlockTest)
                {
                    continue;
                }
                if (s.DeadlockTestEnabled)
                {
                    TestTimeouts(freq);
                    if (!_active)  // else interval elapsed
                        if (!possibleDeadlock)
                            possibleDeadlock = true;
                        else
                        {
                            _deadlock = true;
                            // well, maybe
                            // so test state of components
                            _msgs = new List<String>();
                            _msgs.Add("Network has deadlocked");
                            if (ListCompStatus(_msgs))
                            {
                                //          interruptAll();
                                foreach (string m in _msgs)
                                    Console.Out.WriteLine(m);
                                Console.Out.WriteLine("*** Deadlock detected in Network ");
                                Console.Out.Flush();

                                // terminate the net instead of crashing the application
                                Terminate();
                                // tell the caller a deadlock occurred
                                FlowError.Complain("Deadlock detected in Network");
                                break;
                            }
                            // one or more components haven't started or
                            // are in a long wait
                            _deadlock = false;
                            possibleDeadlock = false;
                        }
                    _active = false;
                }
            }

            if (_deadlock) InterruptAll();
            else
            {

                foreach (Component c in _components.Values)
                { if (c._thread != null)                    
                    c._thread.Join(); }

                //if (outputCopier != null)
                //   outputCopier.Join();
            }

        }
        /* not needed any more
        bool DeadlockTest()
        {
            List<string> msgs = new List<string>();
            // Messages are added to list, rather than written directly,
            //   in case it is not a deadlock
            msgs.Add("Network has deadlocked");
            lock (StateSynchRoot)
            {
                foreach (Component comp in _components.Values)
                {
                    if (comp.Status == States.Active ||
                        comp.Status == States.LongWait) return false;
                    msgs.Add(String.Format("--- {1,-13} -- {0}", comp.Name, comp.Status));
                }
                foreach (string m in msgs)
                    Console.Out.WriteLine(m);
                //FlowError.Complain("Deadlock detected");
                System.Diagnostics.Trace.Fail("*** Deadlock detected in Network ");
                return true;
            }
        }
        */

        /**
   * Queries the status of the subnet's components.
   * if deadlock, return true, else return false   
   * 
   * @param msgs the message vector for status lines
   */

        bool ListCompStatus(List<String> msgs)
        {
            bool terminated = true;
            lock (this)
            {
                foreach (Component comp in _components.Values)
                {
                    if (comp is SubNet)
                    {
                        SubNet subnet = comp as SubNet;
                        if (!subnet.ListCompStatus(msgs))
                            return false;
                    }
                    else
                    {
                        if (comp._status == States.Active || comp._status == States.LongWait)
                        {
                            return false;
                        }

                        if (comp.Status != States.Terminated)
                            terminated = false;
                        string st = Enum.GetName(typeof(States), comp._status);
                        st = (st + "            ").Substring(0, 13);
                        msgs.Add(String.Format("--- {1}     {0}", Name + "." + comp.Name, st));
                    }
                }


                return !terminated;
            }
        }
        // called by WaitForAll method
        void TestTimeouts(int freq)
        {
            lock (_timeouts)
            {
                foreach (TimeoutHandler t in _timeouts.Values)
                {
                    t.Decrement(freq);
                }
            }
        }
        internal virtual void InterruptAll()
        {

            IDictionaryEnumerator all = _components.GetEnumerator();
            while (all.MoveNext())
            {
                Component c = (Component)(all.Value);
                if (!c.IsTerminated())
                {
                    c._thread.Interrupt();
                    Console.Out.WriteLine(c.Name + " interrupted");
                }

            }
        }
        internal void NotifyTerminated(Component comp)
        {
            // lock (StateSynchRoot)
            // {
            //   foreach (Component comp in _components.Values)
            //   {
            //      if (comp.Status != States.Terminated)
            //          return;
            //  }
            //_mainthread.Interrupt(); 
            lock (comp)
            {
                comp._status = States.Terminated;
            }
            Trace("Terminated: " + comp.Name);
            _cdl.CountDown();
            // }
        }
        internal override void Terminate(States newStatus)
        {
            // prevent deadlock testing, components will be shut down anyway
            _abort = true;
            foreach (Component comp in _components.Values)
            {
                comp.Terminate(newStatus);
            }
        }
        /**
           * Handles errors in the network.
           * @param e the exception which specifies the error
           */
        public virtual void SignalError(Exception e)
        {
            // only react to the first error, the others presumably are inherited errors
            if (_error == null)
            {
                // set the error field to let go() throw the exception
                _error = e;
                // terminate the network's components
                foreach (Component comp in _components.Values)
                {
                    comp.Terminate(States.Error);
                }
            }
        }
        /**
   * Shuts down the network.
   */
        public void Terminate()
        {
            Terminate(States.Terminated);
        }

        /* Generate Trace line */
        internal void Trace(string msg)
        {
            if (_tracing)
            {
                lock (this)
                {
                    DateTime now = DateTime.UtcNow;

                    string dt = now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                    string n = GetTracingName();

                    // forceConsole is used for debugging purposes to force writing to the console
                    // useConsole will be set to true if the trace file could not be opened
                    if (_forceConsole || _useConsole)
                    {
                        lock (_network)
                        {
                            Console.Out.WriteLine(dt + " " + n + ": " + msg);
                            Console.Out.Flush();
                        }
                        return;
                    }
                    FileStream fs = null;
                    if (_traceWriter == null)
                    {
                        string s = _tracePath + n + "-fulltrace.txt";
                        // Delete the file if it exists.
                        if (File.Exists(s))
                        {
                            File.Delete(s);
                        }
                        try
                        {
                            fs = new FileStream(s, FileMode.OpenOrCreate, FileAccess.Write);
                            _traceWriter = new StreamWriter(fs);
                        }
                        catch (IOException e)
                        {
                            // file cannot be created or opened - disable tracing
                            // _tracing = false;
                            lock (_network)
                            {
                                Console.Out.WriteLine("Trace file " + s + " could not be opened - \n" +
                                "   writing to console...");
                                Console.Out.WriteLine(dt + " " + n + ": " + msg);
                                Console.Out.Flush();
                            }
                            _useConsole = true;
                            return;
                        }
                        _traceFileList.Add(_traceWriter);
                    } try
                    {
                        _traceWriter.WriteLine(dt + " " + msg);
                        _traceWriter.Flush();
                        // Thread.Sleep(500);  // for testing
                    }
                    catch (IOException e)
                    {
                        Console.Out.WriteLine("Trace file " + fs.Name + " could not be written - \n" +
                                "   writing to console...");
                        Console.Out.WriteLine(dt + " " + n + ": " + msg);
                        Console.Out.Flush();
                    }

                }
            }
        }

        void CloseTraceFiles()
        {
            foreach (StreamWriter x in _traceFileList)
                x.Close();
        }
        string GetTracingName()
        {
            if (_mother != null)
            {
                // we're in a component or in a subnet
                // recursively build the tracing name
                return _mother.GetTracingName() + "." + Name;
            }
            // we're in the Network class
            return Name; // or "Network"

        }

        internal void Trace(string format, params object[] args)
        {
            Trace(String.Format(format, args));
        }

        /**
  * Sets a new path for trace files. By default the current directory will be
  * used.
  * @param path the trace path to set
  */
        public static void SetTracePath(string path)
        {
            if (path.EndsWith(@"\") || path.Equals(""))
            {
                Network._tracePath = path;
            }
            else
            {
                // append the file name separator if it is missing and the path is not
                // empty
                Network._tracePath = path + @"\";
            }
        }
    }
}
