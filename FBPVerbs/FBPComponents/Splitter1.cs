using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;

namespace Components
{
    [InPort("IN")] 
    [OutPort("OUT", arrayPort = true)]
    [ComponentDescription("Read IN, divide into sections of 30 packets to OUT array")]
    public class Splitter1 : Component
    {
        internal static string _copyright =
        "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
        "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
        "based on the Everything Development Company's Artistic License.  A document describing " +
        "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
        "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        IInputPort _inp;
        OutputPort[] _outps;
        /*
        public override object[] Introspect()
        {
            return new object[] {
                "Read IN, divide into sections of 30 packets to OUT array"
                
            };
        }
        */
        public override void OpenPorts()
        {
            _inp = OpenInput("IN");
            _outps = OpenOutputArray("OUT");
        }

        // copy 30 packets at a time to consecutive output ports
        // close each port as it is finished
        // send all excess output to the last port
        public override void Execute()
        {
            int i = 0;
            int count = 0;
            for (Packet p = _inp.Receive(); p != null; p = _inp.Receive())
            {
                // Console.Out.WriteLine("{0} {1}", count, i);
                _outps[i].Send(p);
                
                if (++count == 30 && i != _outps.Length - 1)
                {
                    _outps[i].Close();
                    i++;
                    count = 0;
                }
                
            }

        }
        
    }
}
