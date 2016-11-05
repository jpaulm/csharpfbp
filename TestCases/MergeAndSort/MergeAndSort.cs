using System;
using System.IO;
using Components;
using FBPLib;


namespace TestNetworks
{
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
            Component("ProcessWRandDelays", typeof(ProcessWRandDelays));
            Component("ProcessWRandDelays2", typeof(ProcessWRandDelays));
            Component("Sort", typeof(Sort));
            Component("Write", typeof(WriteToConsole));

            Connect(Component("Generate"),
                Port("OUT"),
                Component("ProcessWRandDelays"),
                Port("IN"));

            Connect("ProcessWRandDelays2.OUT", "ProcessWRandDelays.IN");
            Initialize("120",
                Component("Generate"),
                Port("COUNT"));

            Connect(Component("Generate2", typeof(GenerateTestData)),
                Port("OUT"),
                Component("ProcessWRandDelays2"),
                Port("IN"));

            Connect("ProcessWRandDelays.OUT", "Sort.IN");

            Initialize("75",
                Component("Generate2"),
                Port("COUNT"));

            Connect(Component("Sort"),
                Port("OUT"),
                Component("Write"),
               Port("IN"));

          

        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            // Additional calls to MergeAndSort.Go() were starting before the previous ones had destroyed all threads - 
            //  this seems to differ from JavaFBP, since the Network.Go methods are basically the same in
            //  both implementations.  

            // I have therefore added a Monitor.Enter call in Network.Go to a static lock object called _netObject.  
            // This seems to have fixed the problem.

            for (int i = 0; i <10; i++)
                new MergeAndSort().Go();
            //Console.Read();
        }
        
    }
}


