using System;
using System.IO;
using FBPLib;
using Components;


namespace TestNetworks.Networks
{
    /** This network forces a deadlock condition */

    public class Deadlock : Network
    {

    /* *
          * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
          * distribute, or make derivative works under the terms of the Clarified Artistic License, 
          * based on the Everything Development Company's Artistic License.  A document describing 
          * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
          * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
          * */


        public override void Define() /* throws Throwable */ {
		Connect(Component("Read", typeof(ReadText)),
			Port("OUT"),
			Component("Display", typeof(WriteToConsole)),
			Port("IN"));
        Object d = (Object)@"..\..\mfile";
        Initialize(d,
			Component("Read"),
			Port("SOURCE"));
        Connect(Component("Display"),
            Port("OUT"),
            Component("ReplString", typeof(ReplString)),
            Port("IN"));

            Connect(Component("ReplString"),
			Port("OUT", 0),
			Component("Concatenate", typeof(Concatenate)),
			Port("IN", 0));
		
		Connect(Component("Concatenate"),
			Port("OUT"),
			Component("Discard", typeof(Discard)),
			Port("IN"));

		Connect(Component("ReplString"),
			Port("OUT", 2),
			Component("Concatenate"),
			Port("IN", 1));
		
		Connect(Component("ReplString"),
			Port("OUT", 1),
			Component("Concatenate"),
			Port("IN", 2));
		}
	internal static void main(String[] argv) {
		new Deadlock().Go();
		}
}

}
