
namespace FBPLib
{
    using System;
    using System.Collections;
    using System.Threading;


    /// <summary>This class implements buffering between Component threads.
    /// One is created behind the scenes whenever two ports are connected.
    /// </summary>

    public class Connection : IInputPort
    {
        /* *
         * Copyright 2007, ..., 2011, J. Paul Morrison.  At your option, you may copy,
         * distribute, or make derivative works under the terms of the Clarified Artistic License,
         * based on the Everything Development Company's Artistic License.  A document describing
         * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm.
         * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
         * */
        internal PacketBuffer _buffer;
        // The unique receiver.  We need to activate() it whenever
        // packets arrive, or if connection is closed and receiver is IMustRun.
        public Component _receiver;

        // The Component who happens to be sending on a send()
        internal Component _sender;

        // Number of senders who have called setSender() but are not closed.
        internal volatile int _senderCount = 0;

        // The list of types (Class objects) that senders have declared.
        // Vector senderTypes;

        // The type that the receiver has declared.
        //internal System.Type _receiverType;
        // internal bool partial = true; //set by NewComponent
        // internal bool arrayPort;  // used when processing InPort attribute
        //internal bool triggering;	

        private Network _traceNetwork; // the network this port is visible in (needed for tracing)

        internal string _name;
        internal string _fullName;
        internal bool _IPCount;
        private bool _dropOldest;
        public string Name
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
        public Component Receiver
        {
            get
            {
                return _receiver;
            }
            set
            {
                _receiver = value;
            }

        }

        /// <summary>Constructor: make a new connection of a given size
        /// </summary>
        internal Connection(int size)
        {
            _buffer = new PacketBuffer(size);
        }
        /// <summary>Invoked to tell us we have a(nother) sender.
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'bumpSenderCount'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        internal void BumpSenderCount()
        {
            lock (this)
            {
                _senderCount++;
            }
        }
        /// <summary>Returns the number of packets this Connection can hold.
        /// </summary>
        public int Capacity()
        {
            return _buffer.Capacity();
        }
        /*
        /// <summary> Throws an error if the input object types and output object types do not match
        /// (This needs some more thinking about...)
        /// </summary>
        public void CheckTypes()
        {

            return;
			
            if (senderTypes == null) return;
            if (receiverType == Object.class) return;
            Enumeration types = senderTypes.elements();
            while (types.hasMoreElements()) {
            Class senderType = (Class)(types.nextElement());
            if (!(receiverType.isAssignableFrom(senderType)))
            FlowError.complain("Connection type mismatch");
            }
			
			 
        }
         */
        /// <summary>The close input connection function.
        /// </summary>

        //UPGRADE_NOTE: Synchronized keyword was removed from method 'close'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        public void Close()
        {
            lock (this)
            {
                Trace("Close connection");

                if (IsClosed())
                    return;
                _senderCount = 0; // set sender count to zero
                if (_buffer.Count() > 0)
                    Console.Out.WriteLine(_buffer.Count() + " packets on input connection lost");

                System.Threading.Monitor.PulseAll(this); // wakes up any senders waiting for slots

            }
        }
        /// <summary> Close the sending Component
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'closeSender'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        internal void IndicateOneSenderClosed()
        {
            //lock ((_receiver._inputPorts as ICollection).SyncRoot)
            //{
            try
            {
                Monitor.Enter(_receiver._lockObject);
                lock (this)
                {
                    if (!IsClosed())
                    {
                        --_senderCount;
                        if (IsDrained())
                        {
                            if (_receiver.Status == Component.States.Dormant ||
                                _receiver.Status == Component.States.NotStarted)
                                _receiver.Activate();
                            else
                                System.Threading.Monitor.PulseAll(this);
                        }
                    }
                }
            }
            finally
            {
                Monitor.Exit(_receiver._lockObject);
            }
        }
        /// <summary>Return the number of packets currently in this Connection.
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'count'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        public int Count()
        {
            lock (this)
            {
                return _buffer.Count();
            }
        }
        public string GetName()
        {
            return Name;
        }
        /// <summary>Returns true if this connection is closed (not necessarily drained).
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'IsClosed'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        internal bool IsClosed()
        {
            lock (this)
            {
                return (_senderCount == 0);
            }
        }
        /// <summary>Returns true if this connection is drained (closed and empty).
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'isDrained'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        internal bool IsDrained()
        {
            lock (this)
            {
                return (_senderCount == 0 && _buffer.Count() == 0);
            }
        }
        /// <summary>Returns true if this connection is empty
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'isEmpty'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        internal bool IsEmpty()
        {
            lock (this)
            {
                return (_buffer.Count() == 0);
            }
        }
        /// <summary>Returns true if this connection is full.
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'isFull'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        internal bool IsFull()
        {
            lock (this)
            {
                return (_buffer.Count() == _buffer.Capacity());
            }
        }

        /// <summary>The receive function.
        /// See IInputPort.receive.
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'receive'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        public Packet Receive()
        {
            lock (this)
            {
                Trace("Receiving");
                

                if (IsDrained())
                {

                    Trace("Recv/close");
                    return null;
                }
                Interlocked.Increment(ref Network.receives);

                while (IsEmpty())
                {
                    _receiver.Status = Component.States.SuspRecv;

                    Trace("Recv/susp");

                    try
                    {
                        System.Threading.Monitor.Wait(this);
                    }
                    catch (System.Threading.ThreadInterruptedException e)
                    {
                        //UPGRADE_NOTE: Exception 'java.lang.ThreadDeath' was converted to ' ' which has different behavior. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1100"'
                        //      throw new System.ApplicationException();
                        Close();
                        FlowError.Complain(_receiver._name + ": Interrupted");
                        // unreachable
                        return null;
                    }

                    //if (Component._network._deadlock)
                   // {
                    //    _receiver._thread.Interrupt();
                    //    Trace("Receive interrupted because of deadlock");
                    //    return null;
                   // }

                    
                    _receiver.Status = Component.States.Active;
                    Trace("Recv/resume");
                    if (IsDrained())
                    {
                        Trace("Receive drained");
                        return null;
                    }
                }

                if (IsDrained())
                {
                    Trace("Receive drained");
                    return null;
                }

                if (IsFull())
                    System.Threading.Monitor.PulseAll(this);
                // if was full, notify any Components waiting to send

                Packet packet = _buffer.Take();

                packet.Owner = _receiver;

                if (null == packet.Content)

                    Trace("Receive OK");
                else

                    Trace("Receive OK: " + packet);
                _receiver._network._active = true;
                return packet;
            }
        }

        /// <summary>The send function.  See OutputPort.send.
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'send'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        internal bool Send(Packet packet, OutputPort op)
        {
            lock (this)
            {
                if (packet == null)
                    throw new System.ArgumentException();
                _sender = op._sender;

                if (IsClosed())
                {
                    return false;
                }
                //_sender._mother.Trace(Name + ": Sending: " + packet);

                
                if (IsEmpty())
                    System.Threading.Monitor.PulseAll(this);

                while (IsFull())
                {
                    if (_dropOldest)
                    {
                        Packet p = _buffer.Take();
                        Interlocked.Increment(ref Network.dropOlds);
                        _sender._mother.Trace("{0}: DropOldest", _sender.Name);                    
                    }
                    else
                    {
                        _sender.Status = Component.States.SuspSend;
                        
                        _sender._mother.Trace("{0}: Send/susp", _sender.Name);
                        try
                        {
                            System.Threading.Monitor.Wait(this);
                        }
                        catch (System.Threading.ThreadInterruptedException e)
                        {
                            //UPGRADE_NOTE: Exception 'java.lang.ThreadDeath' was converted to ' ' which has different behavior. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1100"'

                            //      throw new System.ApplicationException();
                            IndicateOneSenderClosed();
                            FlowError.Complain(_sender._name + ": interrupted");
                            // unreachable code
                            return false;
                        }
                        // if (Component._network._deadlock)
                        //{
                        //    _sender._thread.Interrupt();
                        //    _sender._mother.Trace(_sender._name + ": Send interrupted because of deadlock");
                        //    // unreachable code
                        //    return false;
                        // }

                        _sender = op._sender;
                        _sender.Status = Component.States.Active;

                        _sender._mother.Trace("{0}: Send/resume", _sender.Name);
                    }
                }
                if (IsClosed())
                {
                    _sender._mother.Trace("{0}: Send/close", _sender.Name);
                    //Interlocked.Increment(ref Network.sends);
                    return false;
                }

                //lock ((_receiver._inputPorts as ICollection).SyncRoot)
                try
                {
                    Monitor.Enter(_receiver._lockObject);
                    packet.ClearOwner();
                    _buffer.Put(packet);

                    if (_receiver.Status == Component.States.Dormant ||
                        _receiver.Status == Component.States.NotStarted)
                        _receiver.Activate();
                    else
                        System.Threading.Monitor.PulseAll(this);

                    _sender._network._active = true;
                    _sender = null;

                }
                catch (ThreadInterruptedException e)
                {
                    return false;
                }
                finally
                {
                    Monitor.Exit(_receiver._lockObject);
                }
                Interlocked.Increment(ref Network.sends);
                return true;
            }
        }

        public void SetDropOldest()
        {
            _dropOldest = true;
        }


        /// <summary>Invoked to tell us we have a receiver.
        /// </summary>

        public void SetReceiver(Component newReceiver)
        {
            //_receiver = newReceiver;
            if (_receiver == null)
            {
                // called by Network.connect()
                _receiver = newReceiver;
                _traceNetwork = newReceiver._mother;
            }
            else
            {
                // always use the same lock for subnet ports
                newReceiver._lockObject = _receiver._lockObject;
                _receiver = newReceiver;
            }
        }
        /**
   * Issues tracing messages belonging to this input port. 
   */
        void Trace(string msg)
        {
            _traceNetwork.Trace(_fullName + ": " + msg);
        }

        /*
        /// <summary>Invoked to tell us the type of packet content being sent
        /// or expected.  The receiver's type must be a supertype of
        /// every sender's type, or the Network is ill-formed.
        /// </summary>
        public void SetType(Type type)
        {

            if (type == null) return;

            _receiverType = type;

        }
         */

    }
}

