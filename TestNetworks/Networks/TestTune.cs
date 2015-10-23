using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FBPLib;
using Components;

namespace TestNetworks.Networks
{
    public class TestTune : Network
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
            Component("Read", typeof(ReadText));
            Component("Tune", typeof(Tune));
            Component("Text2IntArray", typeof(Text2IntArray));
            Connect("Read.OUT", "Text2IntArray.IN");
            Connect("Text2IntArray.OUT", "Tune.IN");

            Object d = (Object)@"..\..\tune.txt";
            Initialize(d,
                Component("Read"),
                Port("SOURCE"));


        }
    }
}

