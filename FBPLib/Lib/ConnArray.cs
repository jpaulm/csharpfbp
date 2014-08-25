namespace FBPLib
{

    using System;
    using System.Collections;
    using System.Threading;


    /// <summary>This class represents a Connection Array
    /// </summary>

    public class ConnArray : IInputPort
    {
        /* *
         * Copyright 2007, ..., 2011, J. Paul Morrison.  At your option, you may copy,
         * distribute, or make derivative works under the terms of the Clarified Artistic License,
         * based on the Everything Development Company's Artistic License.  A document describing
         * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm.
         * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
         * */
        internal bool _fixedSize;

        internal string _name;
        Component _receiver;
        public bool _optional = false;
        
        public int Count()
        {
            return 0;
        }
        public void Close()
        {
            return;
        }
        public int Capacity()
        {
            return 0;
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
        public Packet Receive()
        {
            return null;
        }
        public void SetReceiver(Component newReceiver)
        {
            _receiver = newReceiver;
       
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
    }
}