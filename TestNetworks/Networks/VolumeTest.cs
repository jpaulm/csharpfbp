using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
    /** This network is intended for timing runs */


    public class VolumeTest : Network
    {

    /* *
           * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
           * distribute, or make derivative works under the terms of the Clarified Artistic License, 
           * based on the Everything Development Company's Artistic License.  A document describing 
           * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
           * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
           * */

        public override void Define() /* throws Throwable */ {
		Connect(Component("Generate", typeof(GenerateTestData)),
			Port("OUT"),
			Component("ReplString ", typeof(ReplString)),
			Port("IN"));
		Connect(Component("ReplString "),
			Port("OUT", 0),
			Component("ReplString 2", typeof(ReplString)),
			Port("IN"));
		Connect(Component("ReplString 2"),
			Port("OUT", 0),
			Component("ReplString 3", typeof(ReplString)),
			Port("IN"));
		Connect(Component("ReplString 3"),
			Port("OUT", 0),
			Component("ReplString 4", typeof(ReplString)),
			Port("IN"));
		Connect(Component("ReplString 4"),
			Port("OUT", 0),
			Component("ReplString 5", typeof(ReplString)),
			Port("IN"));
		Connect(Component("ReplString 5"),
			Port("OUT", 0),
			Component("ReplString 6", typeof(ReplString)),
			Port("IN"));
		Connect(Component("ReplString 6"),
			Port("OUT", 0),
			Component("ReplString 7", typeof(ReplString)),
			Port("IN"));
		Connect(Component("ReplString 7"),
			Port("OUT", 0),
			Component("ReplString 8", typeof(ReplString)),
			Port("IN"));
		
		Connect(Component("ReplString 8"),
			Port("OUT", 0),
            Component("Discard", typeof(Discard)),
			Port("IN"));
        
		Initialize("10000",
			Component("Generate"),
			Port("COUNT"));
		
	
		}
	internal static void main(String[] argv) {
		new VolumeTest().Go();
		}
}

}
