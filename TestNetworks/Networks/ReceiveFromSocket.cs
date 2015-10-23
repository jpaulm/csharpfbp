using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
    public class ReceiveFromSocket : Network
    {
        /* *
            * Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, 
            * distribute, or make derivative works under the terms of the Clarified Artistic License, 
            * based on the Everything Development Company's Artistic License.  A document describing 
            * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
            * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
            * */
        public override void Define()        {
                       
           // Component("Generate", typeof(Generate));
          //  Component("WS", typeof(WriteToSocket));
            Component("RS", typeof(ReadFromSocket));
            Component("Display", typeof(WriteText));
            
          //  Connect("Generate.OUT", "WS.IN");
            Connect("RS.OUT", "Display.IN");
            
          //  Initialize("100",
           //     Component("Generate"),
          //      Port("COUNT"));

            Stream st = Console.OpenStandardOutput();
            Initialize(st,
                Component("Display"),
                Port("DESTINATION"));


         //   Initialize("4444",
          //      Component("WS"),
          //      Port("PORT"));
            Initialize("4444",
                Component("RS"),
                Port("PORT"));
            
            

        }
    }
}
