using System;
using System.IO;
using FBPLib;
using Components;


namespace TestNetworks.Networks
{
    public class TestMixedInput : Network
    {

        /* *
             * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
             * distribute, or make derivative works under the terms of the Clarified Artistic License, 
             * based on the Everything Development Company's Artistic License.  A document describing 
             * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
             * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
             * */


        public override void Define() /* throws Throwable */ {
		/* component("MONITOR", Monitor.class); */
        // Component("Inject", typeof(Inject));
        Connect(Component("Read", typeof(ReadText)),
			Port("OUT"),
            Component("Splitter1", typeof(Splitter1)),
			Port("IN"));
        Object d = (Object) "Data\\myXML3.txt";
        Initialize(d,
			Component("Read"),
			Port("SOURCE"));
		
		Connect(Component("Splitter1"),
			Port("OUT", 0),
            Component("Concatenate", typeof(Concatenate)),
			Port("IN", 0));
		
		Connect(Component("Concatenate"),
			Port("OUT"),
            Component("Write", typeof(WriteToConsole)),
			Port("IN"));

		Connect(Component("Splitter1"),
			Port("OUT", 1),
			Component("Concatenate"),
			Port("IN", 2));
		
	    Object s = (Object) "ABCDEF";
        Initialize(s,
			Component("Concatenate"),
            Port("IN", 1));
		
	    //Stream st = Console.OpenStandardOutput();
		//Initialize(st,
		//	Component("Write"),
		//	Port("DESTINATION"));	
		
		
		}
	internal static void main(String[] argv) {
		new TestMixedInput().Go();
		}
}

}
