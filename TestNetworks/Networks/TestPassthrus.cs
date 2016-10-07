using System;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{

    /** This network is intended to test going in and out of dormant state */

    public class TestPassthrus : Network
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

            Component("Generate", typeof(GenerateTestData));
            Component("P1", typeof(ProcessWRandDelays));
            Component("P2", typeof(Passthru));
            Component("P3", typeof(ProcessWRandDelays));
            Component("P4", typeof(Passthru));
            Component("C1", typeof(Copy));
            Component("C2", typeof(Copy));
            Component("C3", typeof(Copy));
            Component("C4", typeof(Copy));
            Component("Discard", typeof(Discard));
            Initialize("1000", "Generate.COUNT");
            Connect("Generate.OUT", "P1.IN");
            Connect("P1.OUT", "C1.IN");
            Connect("C1.OUT", "P2.IN");
            Connect("P2.OUT", "C2.IN");
            Connect("C2.OUT", "P3.IN");
            Connect("P3.OUT", "C3.IN");
            Connect("C3.OUT", "P4.IN");
            Connect("P4.OUT", "C4.IN");
            Connect("C4.OUT", "Discard.IN");

        }
        internal static void main(String[] argv)
        {
            new TestPassthrus().Go();
        }
    }
}
