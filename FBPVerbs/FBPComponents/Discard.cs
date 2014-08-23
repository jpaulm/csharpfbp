using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;


namespace Components
{
    /** 
     * Component to discard all incoming packets 
    */

    [InPort("IN")]
    [ComponentDescription("Discard incoming packets")]
    
    public class Discard : Component
    {

        internal static string _copyright =
           "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
           "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
           "based on the Everything Development Company's Artistic License.  A document describing " +
           "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
           "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;
        

        // make it a non-looper 
        public override void Execute() /* throws Throwable */  {
            Packet p = _inport.Receive();

            Drop(p);


        }
        /*
        public override Object[] Introspect()
        {
            return new Object[] {
		"discards its input",
		"IN", "input", null,
			"packets to pass through"};
        }
        */
        public override void OpenPorts()
        {
            _inport = OpenInput("IN");
            //_inport.SetType(Type.GetType("System.Object"));
            
        }
    }
}
