using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;


namespace   Components
{
    /** Component to pass through all incoming packets - mostly used for
    * debugging.
    */

    [InPort("IN")]
    [OutPort("OUT")]
    [ComponentDescription("Copies its input to its output")]

    public class Copy : Component
    {

        internal static string _copyright =
           "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
           "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
           "based on the Everything Development Company's Artistic License.  A document describing " +
           "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
           "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;
        OutputPort _outport;

        // make it a non-looper 
        public override void Execute() /* throws Throwable */  {
            Packet p;
            for (p = _inport.Receive(); p != null; p = _inport.Receive())
                _outport.Send(p);
        }
        /*
        public override Object[] Introspect()
        {
            return new Object[] {
		"passes its input to its output",
		"IN", "input", null,
			"packets to pass through",
        "OUT", "output", null,
			"destination for packets"};
        }
        */
        public override void OpenPorts()
        {
            _inport = OpenInput("IN");
            //_inport.SetType(Type.GetType("System.Object"));
            _outport = OpenOutput("OUT");
        }
    }
}
