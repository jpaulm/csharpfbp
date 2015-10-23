using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
    [OutPort("OUT")] 
    [InPort("IN")] 
	[InPort("READFILE")]
	[InPort("WRITEFILE")]

    public class InfiniteQueue : SubNet
    {
        /* *
            * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
            * distribute, or make derivative works under the terms of the Clarified Artistic License, 
            * based on the Everything Development Company's Artistic License.  A document describing 
            * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
            * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
            * */
        public override void Define()
        {
            
            Component("SubIn", typeof(SubIn));
            Component("SubInD", typeof(SubIn));
            Component("SubInS", typeof(SubIn));
            Component("Write", typeof(WriteText));
            Component("SubOut", typeof(SubOut));
            Component("Read", typeof(ReadText));

            Connect("SubIn.OUT", "Write.IN");
            Connect("Read.OUT", "SubOut.IN");
            Connect("Write.*", "Read.*");
            Connect("SubInD.OUT", "Write.DESTINATION");
            Connect("SubInS.OUT", "Read.SOURCE");

            //Initialize(Filename, "Write.DESTINATION");
            //Initialize(Filename, "Read.SOURCE");

            Initialize("IN", "SubIn.NAME");
            Initialize("OUT", "SubOut.NAME");
            Initialize("WRITEFILE", "SubInD.NAME");
            Initialize("READFILE", "SubInS.NAME");

        }
    }
}
