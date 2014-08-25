namespace FBPLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary> This class is used within Components to declare instance variables
    /// that hold output ports.  Such instance variables should be assigned
    /// within the <code>openPorts</code> routine of the Component and never
    /// changed thereafter.  Packets can be sent, and the status of the
    /// port manipulated, using the API specified by this interface.
    /// *
    /// </summary>

    public class OutputPort
    {
        /* *
        * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
        * distribute, or make derivative works under the terms of the Clarified Artistic License, 
        * based on the Everything Development Company's Artistic License.  A document describing 
        * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
        * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
        * */
       // internal Component Sender
       // {
        //    set
        //    {
        //        _sender = value;
        //                  }
      //  }
        /*
        internal System.Type Type
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

        internal static string copyright = "Copyright 1999,..., 2011, J. Paul Morrison.  At your option, you may copy, " + "distribute, or make derivative works under the terms of the Clarified Artistic License, " + "based on the Everything Development Company's Artistic License.  A document describing " + "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " + "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        internal Connection _cnxt = null; // downstream connection
        internal bool isClosed = false;
        internal bool _connected = false; 
        internal Component _sender; // Component sending to this Output Port
        internal string _name;
        internal bool _optional = false;
        internal Network _traceNetwork; // the network this port is visible in (needed for tracing)
        internal string _fullName;

        internal string Name
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

        /// <summary> Close this OutputPort.  This is a signal that no further packets
        /// will be sent via this OutputPort.  Since more than one OutputPort may feed a given
        /// Connection, this does not necessarily close the Connection.
        /// *
        /// </summary>

        public virtual void Close()
        {

            Trace("Close output port");

            if (!isClosed)
            {
                isClosed = true;
                lock (_cnxt)
                {
                    if (!_cnxt.IsClosed())
                        _cnxt.IndicateOneSenderClosed();
                }
            }
        }
        /// <summary> This method returns the downstream Packet count for a given OutputPort
        /// It is normally only used by Components that do load balancing.
        /// </summary>
        /// <returns>int
        /// 
        /// </returns>
        public int DownstreamCount()
        {            
                return _cnxt.Count();            
        }
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
        public virtual void Send(Packet packet)
        {
            bool res = true;
            Trace("Sending: " + packet.ToString());
            _sender.CheckOwner(packet);

            if (isClosed)
            {

                Trace("Sending - port closed");
                res = false;
            }
            res &= _cnxt.Send(packet, this); // fire up send method on connection

            if (!res)
                FlowError.Complain("Could not deliver packet to " + Name);
           Trace("Sent OK");

            return;
        }

        public bool IsConnected()
        {
            return _connected;
        }
        
        /// <summary> Set sender reference
        /// </summary>
         public void SetSender(Component c)    
         {            
           
            _sender = c;
         }

         /**
* Issues tracing messages belonging to this input port. 
*/
         void Trace(string msg)
         {
             _traceNetwork.Trace(_fullName + ": " + msg);
         }

        /** Invoked to tell us the type of packet content being sent
         * or expected.  The receiver's type must be a supertype of
         * every sender's type, or the Network is ill-formed.
         
        public void SetType(Type type)
        {             
            if (type == null) return;

            if (cnxt.senderTypes == null)
                cnxt.senderTypes = new Vector();
            cnxt.senderTypes.addElement(type);             
        }
        */
    }
}
