using System;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{

    /** This network is intended to test LoadBalance component */

    public class TestLoadBalancer : Network
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

            int multiplex_factor = 10;
          Component("generate", typeof(GenerateTestData));
          Component("display", typeof(WriteToConsole));
          Component("lbal", typeof(LoadBalance));
          Connect("generate.OUT", "lbal.IN");
          Initialize("100", Component("generate"), Port("COUNT"));
          for (int i = 0; i < multiplex_factor; i++) {
            Connect(Component("lbal"), Port("OUT", i), Component("passthru" + i, typeof(Passthru)), Port("IN"));
            Connect(Component("passthru" + i), Port("OUT"), "display.IN");
          }
          //Stream st = Console.OpenStandardOutput();
          //Initialize(st,
          //    Component("display"),
          //    Port("DESTINATION"));
        }
        internal static void main(String[] argv)
        {
            new TestLoadBalancer().Go();
        }
    }
}
