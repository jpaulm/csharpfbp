using System;
using System.IO;
using Components;
using FBPLib;


namespace TestNetworks.Networks
{

    // This version of MergeAndSort is only used by FBPTest
    // The stand-alone version is in the MergeAndSort project

    public class MergeAndSort : Network
    {

        /* *
          * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
          * distribute, or make derivative works under the terms of the Clarified Artistic License, 
          * based on the Everything Development Company's Artistic License.  A document describing 
          * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
          * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
          * */

        public override void Define() /* throws Throwable */ {


            Component("Generate", typeof(GenerateTestData));
            Component("Passthru", typeof(Passthru));
            Component("Passthru2", typeof(Passthru));
            Component("Sort", typeof(Sort));
            Component("Write", typeof(WriteToConsole));

            Connect(Component("Generate"),
                Port("OUT"),
                Component("Passthru"),
                Port("IN"));

            Connect("Passthru2.OUT", "Passthru.IN");
            Initialize("60",
                Component("Generate"),
                Port("COUNT"));

            Connect(Component("Generate2", typeof(GenerateTestData)),
                Port("OUT"),
                Component("Passthru2"),
                Port("IN"));

            Connect("Passthru.OUT", "Sort.IN");

            Initialize("75",
                Component("Generate2"),
                Port("COUNT"));

            Connect(Component("Sort"),
                Port("OUT"),
                Component("Write"),
               Port("IN"));

            //Stream st = Console.OpenStandardOutput();
            //Initialize(st,
            //    Component("Write"),
            //    Port("DESTINATION"));

            //Initialize("-",
            //    Component("Write"),
            //    Port("FLUSH"));
            //Initialize("500",
            //    Component("Write"),
             //   Port("CONFIG"));

        }

        
    }
}


