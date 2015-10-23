using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
    public class Copy1 : Network
    {

        /* *
               * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
               * distribute, or make derivative works under the terms of the Clarified Artistic License, 
               * based on the Everything Development Company's Artistic License.  A document describing 
               * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
               * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
               * */

        public override void Define() /* throws Throwable */ {
		// component("MONITOR", Monitor.class);
        
        Connect(Component("Generate", typeof(GenerateTestData)),
			Port("OUT"),
            Component("Write", typeof(WriteText)),
			Port("IN"));
        
        
		Initialize("100",
			Component("Generate"),
			Port("COUNT"));

        Stream st = Console.OpenStandardOutput();
		Initialize(st,
			Component("Write"),
            Port("DESTINATION"));
        Initialize("", 
            Component("Write"),
            Port("FLUSH"));
		
	
		}
	internal static void main(string[] argv) {
		new Copy1().Go();
		}
}

}
