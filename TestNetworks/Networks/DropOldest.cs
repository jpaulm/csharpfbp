using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
    /** This network is intended to test dropOldest connection attribute */


    public class DropOldest : Network
    {

        /* *
               * Copyright 2007, 2014, J. Paul Morrison.  At your option, you may copy, 
               * distribute, or make derivative works under the terms of the Clarified Artistic License, 
               * based on the Everything Development Company's Artistic License.  A document describing 
               * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
               * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
               * */

        public override void Define() /* throws Throwable */ {
            Connection c = Connect(Component("Generate", typeof(GenerateTestData)),
                Port("OUT"),
                Component("ProcessSlowly", typeof(ProcessSlowly)),
                Port("IN"));
            c.SetDropOldest();

            Connect(Component("ProcessSlowly"),
                Port("OUT"),                
                Component("Display", typeof(Output)),
                Port("IN"));
            

            Initialize("10000",
                Component("Generate"),
                Port("COUNT"));


        }
        internal static void main(String[] argv)
        {
            new DropOldest().Go();
        }
    }

}
