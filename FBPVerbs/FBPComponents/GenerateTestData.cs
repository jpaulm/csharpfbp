

using System;
using FBPLib;




namespace Components
{

    /** Component to generate a stream of 'n' packets, where 'n' is
    * specified in an InitializationConnection.
    */
    [InPort("COUNT", description="Number of packets", type=typeof(System.String))]
    [OutPort("OUT")] 
    [ComponentDescription("Generate stream of packets based on count")]

    public class GenerateTestData : Component
    {

        internal static string _copyright =
            "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
            "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
            "based on the Everything Development Company's Artistic License.  A document describing " +
            "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
            "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        OutputPort _outport;
        IInputPort _count;


        public override void Execute() /* throws Throwable */ {
            Packet ctp = _count.Receive();

            string param = ctp.Content.ToString();
            Int32 ct = Int32.Parse(param);
            Drop(ctp);
            _count.Close();

            for (int i = ct; i > 0; i--)
            {
                string s = String.Format("{0:d6}", i) + "abcd";

                Packet p = Create(s);
                _outport.Send(p);


            }

            // output.close();
            // terminate();
        }
        /*
        public override System.Object[] Introspect()
        {

            return new Object[] {
		"generates a set of Packets under control of a counter" ,
		"OUT", "output", Type.GetType("System.String"),
			"lines generated",
		"COUNT", "parameter", Type.GetType("System.String"),
			"Count of number of entities to be generated"};
        }
        */
        public override void OpenPorts()
        {

            _outport = OpenOutput("OUT");
           // _outport.SetType(Type.GetType("System.String"));

            _count = OpenInput("COUNT");
           // _count.SetType(Type.GetType("System.String"));

        }
    }
}
