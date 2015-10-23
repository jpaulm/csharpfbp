using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBPLib;
using System.Threading;

namespace Components
{
    [InPort("IN", arrayPort = true)]
    [OutPort("OUT")]
    [ComponentDescription("Mergess input streams at array port IN, preserving substreams, and sends them to port OUT")]
    public class SubstreamSensitiveMerge : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";
        IInputPort[] _inportArray;
        OutputPort _outport;

        public override void Execute()
        {
            int no = _inportArray.Length;

            Packet p;
            int i = -1;
            int substream_level = 0;
            while (true)
            {
                if (substream_level == 0)
                {
                    
                    i = FindInputPortElementWithData(_inportArray);
                   
                    // will suspend if all elements empty but not drained
                    if (i == -1) // all elements are drained
                        return;
                }
                p = _inportArray[i].Receive();
                if (p.Type == Packet.Types.Open)
                    substream_level++;
                else if (p.Type == Packet.Types.Close)
                    substream_level--;
                _outport.Send(p);
            }
        }

        public override void OpenPorts()
        {

            _inportArray = OpenInputArray("IN");
            _outport = OpenOutput("OUT");  

        }
    }

}

