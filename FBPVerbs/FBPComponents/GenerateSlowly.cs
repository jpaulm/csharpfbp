using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;
using System.Threading;


namespace Components
{
    [OutPort("OUT")]
    [ComponentDescription("Generate 100 packets slowly")]
    public class GenerateSlowly:Component
    {
        internal static string _copyright =
           "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
           "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
           "based on the Everything Development Company's Artistic License.  A document describing " +
           "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
           "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        OutputPort _outport;
        public override void Execute() /* throws Throwable */ {
           
            for (int i = 0; i < 100; i++)
            {
                
                Packet p = Create(i);
                Thread.Sleep(100);
                _outport.Send(p);


            }

        }
        public override void OpenPorts()
        {

            _outport = OpenOutput("OUT");
            // _outport.SetType(Type.GetType("System.String"));


        }
        /*
        public override System.Object[] Introspect()
        {

            return new Object[] {
		"generates a set of Packets" ,
		"OUT", "output", Type.GetType("System.String"),
			"lines generated"};
        }
        */
    }
}
