using System;
using System.Text;
using FBPLib;
using System.Threading;

namespace Components
{

    [InPort("IN")]
    [ComponentDescription("Play tune")]
    [Priority(ThreadPriority.Highest)]   // as of C#FBP-2.2 
    public class Tune : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;

        // make it a non-looper 
        public override void Execute() /* throws Throwable */  {
            //
            // Note: _thread (used by SetPriority) is null before Execute()
            //
            // SetPriority(ThreadPriority.Highest); -> moved up to attribute
            Thread.Sleep(750);   // 3/4 sec
            Packet p;
            while ((p = _inport.Receive()) != null)
            {
                int[] intArray = p.Content as int[];

                Console.Beep(intArray[0], intArray[1] - 50);
                Thread.Sleep(50);   // 1/20 sec
                Drop(p);
            }
            
        }
        /*
        public override Object[] Introspect()
        {
            return new Object[] {
		"discards its input",
		"IN", "input", null,
			"tune"};
        }
        */
        public override void OpenPorts()
        {
            _inport = OpenInput("IN");
            //inport.SetType(Type.GetType("System.Object"));
            
        }

    }
}

