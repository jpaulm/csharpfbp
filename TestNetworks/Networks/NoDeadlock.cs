using System;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
    /** This network is similar to the one called Deadlock, but has been modified slightly 
so that it doesn't deadlock */

    public class NoDeadlock : Network
    {

    /* *
           * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
           * distribute, or make derivative works under the terms of the Clarified Artistic License, 
           * based on the Everything Development Company's Artistic License.  A document describing 
           * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
           * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
           * */
        public override void Define() /* throws Throwable */ {
	//	component("MONITOR", Monitor.class);
        Component("Passthru", typeof(Passthru));
        Component("Passthru2", typeof(Passthru));
		Connect(Component("Read", typeof(ReadText)),
			Port("OUT"),
			Component("Splitter1", typeof(Splitter1)),
			Port("IN"));
        Object d = (Object)@"..\..\tune.txt";
        Initialize(d,
			Component("Read"),
			Port("SOURCE"));
		
		Connect(Component("Splitter1"),
			Port("OUT", 0),
			Component("Concatenate", typeof(Concatenate)),
			Port("IN", 0));
		
		Connect(Component("Concatenate"),
			Port("OUT"),
            Component("Discard", typeof(Discard)),
			Port("IN"));

		Connect(Component("Splitter1"),
			Port("OUT", 1),
			Component("Passthru"),
			Port("IN"));
        Connect(Component("Passthru"),
            Port("OUT"),
            Component("Concatenate"),
            Port("IN", 1));
		
		Connect(Component("Splitter1"),
			Port("OUT[2]"),
			Component("Concatenate"),
			Port("IN[2]"));
        Connect(Component("Splitter1"),
            Port("OUT[3]"),
            Component("Passthru2"),
            Port("IN"));
        Connect(Component("Passthru2"),
            Port("OUT"),
            Component("Concatenate"),
            Port("IN[3]"));
        Connect(Component("Splitter1"),
            Port("OUT[4]"),
            Component("Concatenate"),
            Port("IN[4]"));
        Connect(Component("Splitter1"),
            Port("OUT[5]"),
            Component("Concatenate"),
            Port("IN[5]"));
        Connect(Component("Splitter1"),
            Port("OUT[6]"),
            Component("Concatenate"),
            Port("IN[6]"));
		}
	static void main(String[] argv) {
        new NoDeadlock().Go();
		}
        
}

}
