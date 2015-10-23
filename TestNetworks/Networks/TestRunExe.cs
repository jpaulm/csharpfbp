using System;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{

    /** This network is intended to test RunExe component */

    public class TestRunExe : Network
    {

        /* *
                    * Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, 
                    * distribute, or make derivative works under the terms of the Clarified Artistic License, 
                    * based on the Everything Development Company's Artistic License.  A document describing 
                    * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
                    * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
                    * */
        public override void Define() /* throws Throwable */ {
            // Component("MONITOR", Monitor));
            // tracing = true;

          
          Component("RE", typeof(RunExe));
          
        }
        internal static void main(String[] argv)
        {
            new TestRunExe().Go();
        }
    }
}
