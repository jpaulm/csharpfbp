using System;
using System.IO;
using FBPLib;

namespace Components
{

    [MustRun]
    [InPort("IN")]
    [ComponentDescription("Display Open or Close brackets, or data packets")]

    public class Output : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;

        public override void OpenPorts()
        {
            _inport = OpenInput("IN");
        }

        public override void Execute()
        {
            Packet p;
            int level = 1;
            while ((p = _inport.Receive()) != null)
            {
                switch (p.Type)
                {
                    case Packet.Types.Open:
                        Console.Out.WriteLine("OPEN(" + level + ")");
                        level++;
                        break;
                    case Packet.Types.Close:
                        level--;
                        Console.Out.WriteLine("CLOSE(" + level + ")");
                        break;
                    default:
                        Console.Out.WriteLine(p.Content);
                        break;
                }
                Drop(p);
            }
        }
        /*
        public override System.Object[] Introspect()
        {

            return new Object[] {
        "displays brackets"};
        }
        */
    }

}



