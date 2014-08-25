using System;
using System.Collections.Generic;
using System.Text;

namespace FBPLib
{

    /// <summary> This interface is used within Components to declare instance variables
    /// that hold input ports.  Such instance variables should be assigned
    /// within the <code>openPorts</code> routine of the Component and never
    /// changed thereafter.  Packets can be received, and the status of the
    /// port manipulated, using the API specified by this interface.
    /// <p> This is a bit misleading, as it is the Connection object which holds IPs, not
    /// the InputPort.
    /// *
    /// </summary>

    public interface IInputPort
    {
        /* *
        * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
        * distribute, or make derivative works under the terms of the Clarified Artistic License, 
        * based on the Everything Development Company's Artistic License.  A document describing 
        * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
        * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
        * */
        /// <summary> The maximum number of packets which this InputPort can hold.
        /// </summary>
        /// <returns>maximum packet count
        /// *
        /// </returns>
        //string GetName();
        string Name {get; set;}
        int Capacity();
        /// <summary> Close Initialization Connection (dummy method)
        /// </summary>
        void Close();
        /// <summary>Return the number of packets currently at this Input Port.
        /// </summary>
        int Count();
        Component Receiver { get; set;}
        void SetReceiver(Component receiver);
        
       
        /// <summary> Receive the next available packet from this InputPort.
        /// The thread is suspended if no packets are currently available.
        /// At the end of input (when all upstream threads have closed their
        /// connected OutputPorts), <code>null</code> is returned.
        /// </summary>
        /// <returns>next packet, <code>null</code> if none
        /// *
        /// </returns>
        Packet Receive();
        /*
        /// <summary> Specify the type of packet content that will be accepted from this
        /// InputPort.  Specifying <code>null</code> is equivalent to specifying
        /// <code>Object.class</code> -- in other words, any packet content is
        /// acceptable.
        /// </summary>
        /// <param name="type">the class of acceptable packet content
        /// *
        /// </param>
        void SetType(System.Type type);
        */

    }
}

