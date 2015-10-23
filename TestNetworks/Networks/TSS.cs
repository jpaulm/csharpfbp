using System;
using System.Collections.Generic;
using System.Text;
using Components;
using FBPLib;
using System.IO;

namespace TestNetworks.Networks
{
    public class TSS : Network
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
            // string filename = @"..\..\..\infqueue.fil";

            Component("Generate", typeof(GenSS));
            Component("Subnet", typeof(SubnetX));
            Component("Display", typeof(WriteText));
            Component("Display2", typeof(WriteText));
            //Component("Discard", typeof(Discard));

            Connect("Generate.OUT", "Subnet.IN");
            Connect("Subnet.OUT", "Display2.IN");
            Connect("Subnet.*SUBEND", "Display.IN");

            Initialize("100",
                Component("Generate"),
                Port("COUNT"));

            Stream st = Console.OpenStandardOutput();
            Initialize(st,
                Component("Display"),
                Port("DESTINATION"));
            Initialize(st,
                Component("Display2"),
                Port("DESTINATION"));

        }
    }
}
