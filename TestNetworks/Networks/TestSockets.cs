using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
    public class TestSockets : Network
    {
        /* *
            * Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, 
            * distribute, or make derivative works under the terms of the Clarified Artistic License, 
            * based on the Everything Development Company's Artistic License.  A document describing 
            * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
            * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
            * */
        public override void Define()        {
                       
            Component("Generate", typeof(GenerateTestData));
            Component("WS", typeof(WriteToSocket));
            Component("RS", typeof(ReadFromSocket));
            Component("WS2", typeof(WriteToSocket));
            Component("RS2", typeof(ReadFromSocket));
            Component("Disp", typeof(Discard));
            Component("Passthru", typeof(Passthru));
            
            Connect("Generate.OUT", "WS.IN");
            // connect WS to RS via socket!
            Connect("RS.OUT", "Passthru.IN");
            Connect("Passthru.OUT", "WS2.IN");
            // connect WS to RS via socket!
            Connect("RS2.OUT", "Disp.IN");
            
            Initialize("10000",
                Component("Generate"),
                Port("COUNT"));

            //Stream st = Console.OpenStandardOutput();
            //Initialize(st,
             //  Component("Display"),
            //    Port("DESTINATION"));

            Initialize("4444",
              Component("WS"),
                Port("PORT"));
            Initialize("4444",
                Component("RS"),
                Port("PORT"));

            Initialize("4445",
              Component("WS2"),
                Port("PORT"));
            Initialize("4445",
                Component("RS2"),
                Port("PORT"));

        }
    }
}
