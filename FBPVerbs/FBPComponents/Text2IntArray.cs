using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;

namespace Components
{
    /** Convert comma-separated text to an array of ints
**/
    [InPort("IN")]
    [OutPort("OUT")]
    [ComponentDescription("Convert comma-separated text to an array of 'int's")]
    public class Text2IntArray : Component
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
            while ((p = _inport.Receive()) != null)
            {
                string text = p.Content as string;

                string[] parts = text.Split(',');
                string note = parts[0].Trim();
                string duration = parts[1].Trim();
                int[] intArray = {Int32.Parse(note),Int32.Parse(duration)};
                Drop(p);
                _outport.Send(Create(intArray));

            }

        }
        /*
        public override Object[] Introspect()
        {
            return new Object[] {
		"Convert comma-separated text to an array of ints",
		"IN", "input", Type.GetType("System.Object"),
			"input stream",
		"OUT", "output", Type.GetType("System.Int32[]"),
			"array of ints"};
        }
        */
        public override void OpenPorts()
        {

            _inport = OpenInput("IN");
           // _inport.SetType(Type.GetType("System.Object"));

            _outport = OpenOutput("OUT");

        }
    }

}
