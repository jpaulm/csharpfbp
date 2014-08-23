namespace FBPLib
{
    using System;
    using System.Collections.Generic;

    /// <summary>This is a package-private class which is just used to hold
    /// chains attached to Packets.  There are no methods here,
    /// since all the work is being done in class Packet.
    /// This could have been an inner class of Packet.
    /// </summary>

    sealed class Chain
    {
        /* *
        * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
        * distribute, or make derivative works under the terms of the Clarified Artistic License, 
        * based on the Everything Development Company's Artistic License.  A document describing 
        * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
        * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
        * */
        //internal Packet Head;
        internal List<Packet> Members = new List<Packet>();
        internal string Name;
        
            internal Chain(string name)
            {
                Name = name;
                //Head = head;
                Members = new List<Packet>();
            }
        }
    }
 