using System;

using FBPLib;


namespace Components
{
   

/** Component to assign incoming packets to the output port that has the
* smallest backlog of packets waiting to be processed.
*/

[InPort("IN")]
    [OutPort("OUT", arrayPort=true)]
    [ComponentDescription("Obtains packets and distributes them to the port of an array port which has the smallest backlog")]
public class LoadBalance : Component {

internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


	IInputPort inport;
	OutputPort[] outportArray;

	public override void Execute()  {

		int no = outportArray.Length;
		Int32 backlog;
		int sel = -1;

	    Packet p;
		while ((p = inport.Receive()) != null) {
			backlog = Int32.MaxValue;
			for (int i = 0; i < no; i++) {
			    int j = outportArray[i].DownstreamCount();
			    if (j < backlog) {
				    backlog = j;
				    sel = i;
			        }
			    }
			    outportArray[sel].Send(p);

			}
        
	}
    /*
    public override Object[] Introspect()
    {
		return new Object[] {
		"obtains packets at port IN and distributes them to array port OUT" +
		" depending on which output port has the smallest backlog" ,
		"IN", "input", null,
			"input stream",
		"OUT", "output", null,
			"multiple output streams"};
		}
    */
    public override void OpenPorts()
    {

		inport = OpenInput("IN");
		//inport.setType(Object.class);

		outportArray = OpenOutputArray("OUT");

		}
}

}
