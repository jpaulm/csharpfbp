using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;
using System.Threading;


namespace Components
{
    [OutPort("OUT")]
    [ComponentDescription("Generate 4 brackets & 2 data packets")]
    public class GenSubStreams : Component
    {
        internal static string _copyright =
           "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
           "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
           "based on the Everything Development Company's Artistic License.  A document describing " +
           "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
           "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        OutputPort outputPort;
        public override void Execute() /* throws Throwable */ {

            outputPort.Send(Create(Packet.Types.Open, "OPEN(1)"));
            outputPort.Send(Create("A"));
            outputPort.Send(Create(Packet.Types.Open, "OPEN(2)"));
            outputPort.Send(Create("B"));
            outputPort.Send(Create(Packet.Types.Close, "CLOSE(2)"));
            outputPort.Send(Create(Packet.Types.Close, "CLOSE(1)"));
           // outputPort.close();
        }
        public override void OpenPorts()
        {

            outputPort = OpenOutput("OUT");
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
