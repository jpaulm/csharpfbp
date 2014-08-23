using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;

namespace Components
{
    /** Replicate a stream of Packets to 'n' output streams, where
*  each Packet points at a String.
* Note: this is a specific variant of the general Replicate verb (q.v.)
**/
    [InPort("IN")] 
    [OutPort("OUT", arrayPort=true)]
    [ComponentDescription("Replicates input stream and sends copies to array port OUT")]
    public class ReplString : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;
        OutputPort[] _outportArray;

        public override void Execute() /* throws Throwable */ {

            int no = _outportArray.Length;

            Packet p;
            long count = 0;
            while ((p = _inport.Receive()) != null)
            {
                ++count;
                string o = (string)p.Content;
                Drop(p);

                for (int i = 0; i < no; i++)
                {
                    string o2 = o + "";

                    Packet p2 = Create(o2);
                    _outportArray[i].Send(p2);
                  
                    // else System.out.println( "Line written " + count + " by" + getName());
                }
            }
            Console.Out.WriteLine("Repl complete. " + Name);
        }
        /*
        public override Object[] Introspect()
        {
            return new Object[] {
		"replicates input stream at port IN and sends them to array port OUT",
		"IN", "input", Type.GetType("System.Object"),
			"input stream",
		"OUT", "output", Type.GetType("System.Object"),
			"multiple output streams"};
        }
        */
        public override void OpenPorts()
        {

            _inport = OpenInput("IN");
           // _inport.SetType(Type.GetType("System.Object"));

            _outportArray = OpenOutputArray("OUT");

        }
    }

}
