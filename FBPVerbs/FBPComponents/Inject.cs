using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;


namespace Components
{
    /** Component to send an IIP to an output port
*/
    [InPort("IN")]
    [OutPort("OUT")]
    [ComponentDescription("Send one IIP to output port")]
    public class Inject : Component
    {

        internal static string _copyright =
        "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
        "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
        "based on the Everything Development Company's Artistic License.  A document describing " +
        "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
        "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";



        IInputPort _inport;
        OutputPort _outport;

        public override void Execute() /* throws Throwable  */{

            Packet p = _inport.Receive();
            _outport.Send(p);
            _inport.Close();

        }
        /*
        public override Object[] Introspect()
        {
            return new Object[] {
		"concatenates input streams at array port IN and sends them to port OUT",
		"IN", "input", Type.GetType("Object"),
			"input stream",
		"OUT", "output", Type.GetType("Object"),
			"output stream"};
        }
        */
        public override void OpenPorts()
        {

            _inport = OpenInput("IN");
            // inport.setType(Type.GetType("Object"));

            _outport = OpenOutput("OUT");

        }
    }

}
