namespace FBPLib
{
    using System;




    /// <summary>This is used in a Network definition when a port is not connected
    /// </summary>

    sealed class NullConnection : IInputPort
    {
        /* *
          * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
          * distribute, or make derivative works under the terms of the Clarified Artistic License, 
          * based on the Everything Development Company's Artistic License.  A document describing 
          * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
          * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
          * */
        public void SetType(System.Type type) { }

        // bool partial = false;
        internal string _name;
        internal Component _receiver; // The receiver to deliver to.
        public bool _optional = false;
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
            set { _receiver = value; }

        }
        public void SetReceiver(Component receiver)
        {
            Receiver = receiver;
        }
        internal NullConnection()
        {
        }
        /// <summary>Return capacity of 0
        /// </summary>

        public int Capacity()
        {
            return 0;
        }
        public void Close()
        {
        }
        public int Count()
        {
            return 0;
        }
        public string GetName()
        {
            return Name;
        }
        internal bool IsClosed()
        {
            return true;
        }
        public Packet Receive()
        {
            return null;
        }
    }
}
