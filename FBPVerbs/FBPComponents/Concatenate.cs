using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;


namespace Components
{
    /** Component to concatenate two or more streams of packets
*/
    [InPort("IN", arrayPort = true)] 
    [OutPort("OUT")]
    [ComponentDescription("Concatenates input streams")]
    public class Concatenate : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";



        IInputPort[] _inportArray;
        OutputPort _outport;

        public override void Execute() /* throws Throwable  */{

            int no = _inportArray.Length;

            Packet p;
            for (int i = 0; i < no; i++)
            {
                while ((p = _inportArray[i].Receive()) != null)
                {
                    _outport.Send(p);
                }

            }
        }
      
        public  override void OpenPorts()
        {

            _inportArray = OpenInputArray("IN");
            // inport.setType(Type.GetType("Object"));

            _outport = OpenOutput("OUT");

        }
    }

}
