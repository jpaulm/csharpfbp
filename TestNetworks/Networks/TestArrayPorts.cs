using System;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
   
    public class TestArrayPorts : Network
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
        
             Component("generate", typeof(GenerateTestData));
          Component("replicate", typeof(ReplString));
          Component("discard", typeof(Discard));
          Connect("generate.OUT", "replicate.IN");
          Connect("replicate.OUT[0]", "discard.IN");
          Initialize("100", "generate.COUNT");
        }
        
    }
}
