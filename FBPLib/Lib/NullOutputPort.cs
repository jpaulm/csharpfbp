namespace FBPLib
{
    using System;
    public class NullOutputPort : OutputPort
    {
        /* *
         * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
         * distribute, or make derivative works under the terms of the Clarified Artistic License, 
         * based on the Everything Development Company's Artistic License.  A document describing 
         * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
         * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
         * */
        internal Component Sender
        {
            set
            {
                _sender = value;
            }
        }
        /*internal System.Type Type
        {
            set
            {
                 
    if (type == null) return;
				
    if (cnxt.senderTypes == null)
    cnxt.senderTypes = new Vector();
    cnxt.senderTypes.addElement(type);
				
    

            }

        }
         * */

        //internal static string copyright = "Copyright 1999, 2000, 2001, 2002, J. Paul Morrison.  At your option, you may copy, " + "distribute, or make derivative works under the terms of the Clarified Artistic License, " + "based on the Everything Development Company's Artistic License.  A document describing " + "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " + "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        //internal Connection _cnxt = null; // downstream connection
        internal new bool isClosed = true;
        //internal Component _sender; // Component sending to this Output Port
        //internal string _name;
        //internal bool optional = false;
        //internal bool arrayPort = false;
        /*
        
        internal  new string Name
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
        */
        /// <summary> Close the NullOutputPort.  Does nothing.
        /// </summary>

        public override void Close()
        {
            return;
            
        }
        /// <summary> This method returns the downstream Packet count for a given OutputPort
        /// It is normally only used by Components that do load balancing.
        /// </summary>
        /// <returns>int
        /// 
        /// </returns>
        //internal int downstreamCount()
        // {
        //    return _cnxt.Count();
        // }
        /// <summary> Send a packet to this Port.
        /// The thread is suspended if no capacity is currently available.
        /// If the port or connection has been closed, <code>false</code> is
        /// returned; otherwise, <code>true</code> is returned.
        /// <p> Do not reference the packet after sending - someone else may be
        /// modifying it!
        /// </summary>
        /// <param name="packet">packet to send
        /// </param>
        /// <returns>true if successful
        /// *
        /// </returns>

        // The send function.
        public override void Send(Packet packet)
        {
            //_sender.Drop(packet);  -- do nothing - changed Aug. 5, 2014
        }

        /** Invoked to tell us the type of packet content being sent
         * or expected.  The receiver's type must be a supertype of
         * every sender's type, or the Network is ill-formed.
         */


        internal  void SetType(Type type)
        {

            /*
            if (type == null) return;

            if (cnxt.senderTypes == null)
                cnxt.senderTypes = new Vector();
            cnxt.senderTypes.addElement(type);

            */

        }
    }
}




