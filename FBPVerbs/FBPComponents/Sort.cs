using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;

namespace Components
{
    /** Sort a stream of Packets to an output stream
**/
    [InPort("IN")] 
    [OutPort("OUT")]
    [ComponentDescription("Sort input stream")]
    public class Sort : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;
        OutputPort _outport;

        public override void Execute() /* throws Throwable */ {


            Packet p;
            int i = 0, j, k, n;
            Packet[] array = new Packet[9999];
            while ((p = _inport.Receive()) != null)
            {
                array[i] = p;
                // Console.WriteLine("in: " + p.GetContent());
                ++i;
            }

            Console.Out.WriteLine("No. of elements:" + i);
            j = 0;
            k = i;
            n = k;

            string t = null;

            while (n > 0)
            {
                t = null;
                for (i = 0; i < k; ++i)
                {
                    if (array[i] != null)
                    {
                        string s = (string)array[i].Content;
                        if (t == null || (String.Compare(s, t) < 0))
                        {
                            j = i;
                            t = (string)array[j].Content;
                        }
                    }
                }

                _outport.Send(array[j]);
                array[j] = null;

                --n;

            }

        }
        /*
        public override Object[] Introspect()
        {
            return new Object[] {
		"sorts input stream arriving at port IN and sends result to port OUT",
		"IN", "input", Type.GetType("Packet"),
			"input stream",
		"OUT", "output", Type.GetType("Packet"),
			"output stream"};
        }
        */
        public override void OpenPorts()
        {

            _inport = OpenInput("IN");
           // _inport.SetType(Type.GetType("Packet"));

            _outport = OpenOutput("OUT");
           // _outport.SetType(Type.GetType("Packet"));

        }
    }

}
