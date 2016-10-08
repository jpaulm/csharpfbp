// Copyright 2007-2016, J. Paul Morrison.  
// At your option, you may copy, distribute, or make derivative works under the terms of the 
// Clarified Artistic License, based on the Everything Development Company's Artistic License.  
// A document describing this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
// THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
// Modified by Tom Fox 2016-August

using System;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
    // This network is intended to test the LoadBalance component
    public class TestLoadBalanceWithSubstreams : Network
    {
        public override void Define()
        {
            // Component("MONITOR", Monitor));
            // tracing = true;

            int multiplex_factor = 6;
            Component("generate", typeof(GenSS));
			Component("display", typeof (WriteText));
            Component("lbal", typeof(LoadBalance));
            Component("SSMerge", typeof(SubstreamSensitiveMerge));
            Connect("generate.OUT", "lbal.IN", 4);

            Initialize("100", Component("generate"), Port("COUNT"));

            for (int i = 0; i < multiplex_factor; i++)
            {
                Connect(Component("lbal"), Port("OUT", i), Component("passthru" + i, typeof(ProcessSlowly)), Port("IN"), 1);
                Connect(Component("passthru" + i), Port("OUT"), Component("SSMerge"), Port("IN", i), 1);
            }
            Connect("SSMerge.OUT", "display.IN", 4);

			Stream st = Console.OpenStandardOutput(); 
			Initialize(st,
				Component("display"),
				Port("DESTINATION"));
        }

    }
}
