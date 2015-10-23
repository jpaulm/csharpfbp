using System;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{

    public class TestNestedSubstreams : Network
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

            Component("generate", typeof(GenSubStreams));
            Component("subnet", typeof(SubnetX));
            Component("output", typeof(Output));
            Connect("generate.OUT", "subnet.IN");
            Connect("subnet.OUT", "output.IN");
        }

    }
}
