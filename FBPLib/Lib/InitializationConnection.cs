namespace FBPLib
{
    using System;
    using System.Threading;


    /// <summary>This class provides connections that hold just a single object
    /// at Network setup time.  The <code>initialize</code> statement in
    /// the Network configuration mini-language creates instances of
    /// this class.  It is a degenerate form of Connection.
    /// <p> This class implements a type of parametrization of Components -
    /// the "parameter", which can be any object type, is associated with a port,
    /// and is turned into a Packet when the first receive to that port is issued.
    /// This occurs once per activation of that Component.   From the Component's
    /// point of view, it looks like a normal data stream containing one Packet.
    /// </summary>



    sealed class InitializationConnection : IInputPort
    {
        /* *
         * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
         * distribute, or make derivative works under the terms of the Clarified Artistic License, 
         * based on the Everything Development Company's Artistic License.  A document describing 
         * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
         * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
         * */
        /*
        public void SetType(System.Type type)
        {
            if (type == null)
                return;

            if (type == typeof(System.Object))
                return;

            if (!(type.IsAssignableFrom(_content.GetType())))
                FlowError.Complain("Connection type mismatch");
        }
        */
        internal Component _receiver; // The receiver to deliver to.

        // internal Packet _packet;

        internal System.Object _content; // object passed to it by initialize statement

        internal bool _isClosed = false;
        // bool partial = false;

        internal string _name;
        // public Network network;

        /// <summary>Create an InitializationConnection: requires a content and a receiver.
        /// </summary>

        internal InitializationConnection(System.Object content, Component newReceiver)
        {

            this._content = content; // store object
            _receiver = newReceiver;
        }
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
        internal object Content
        {
            get
            {
                return _content;
            }
        }
        public Component Receiver
        {
            get
            {
                return _receiver;
            }
            set { _receiver = value; }

        }
        /// <summary>The maximum number of packets available in an InitializationConnection must be 1.
        /// </summary>
        public int Capacity()
        {
            return 1;
        }
        internal bool IsClosed() { return _isClosed; }

        /// <summary> Close Initialization Connection
        /// </summary>
        public void Close()
        {
            _isClosed = true;
        }

        /// <summary> (Re)Open Initialization Connection
        /// </summary>
        public void Reopen()
        {
            _isClosed = false;
        }

        /// <summary> Return 1 as number of packets in InitializationConnection.
        /// </summary>
        /// <returns>int
        /// 
        /// </returns>
        public int Count()
        {
            return 1;
        }
        public string GetName()
        {
            return Name;
        }
        public void SetReceiver(Component newReceiver) { // added for subnet
    // support
    _receiver = newReceiver;
  }
        /// <summary>The receive function of an InitializationConection.
        /// Returns null after the packet has been delivered (because packet is set to null).
        /// You get one copy per activation
        /// *
        /// Warning: the object contained in this packet must not be modified.
        /// *
        /// See IInputPort.receive.
        /// </summary>
        public Packet Receive()
        {
            if (IsClosed())
                return null;
            else
            {
                Interlocked.Increment(ref Network.receives);
                Packet p = new Packet(_receiver);
                p._content = _content;
                _receiver._mother.Trace("{0}: IIP received: {1}", Name, p);
                Close();    // is needed!
                return p;
            }
        }
        /// <summary> Invoked to tell us the type of packet content being sent
        /// or expected.  The receiver's type must be a supertype of
        /// content, or the Network is ill-formed.
        /// </summary>
    }
}