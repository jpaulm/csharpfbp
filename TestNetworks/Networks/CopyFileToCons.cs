using System;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{

    public class CopyFileToCons : Network
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
			Component("Write", typeof(WriteText)),
			Port("IN"));
        Object d = (Object)@"..\..\mfile";
        Initialize(d,
			Component("Read"),
			Port("SOURCE"));
        Stream st = Console.OpenStandardOutput();
		Initialize(st,
			Component("Write"),
			Port("DESTINATION"));
		}
	internal static void main(String[] argv) {
		new CopyFileToCons().Go();
		}
}

}
