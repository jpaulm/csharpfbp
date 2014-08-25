using System;
using System.Collections.Generic;
using System.Text;

namespace FBPLib
{

    /** Look after output from subnet - added for subnet support 
     * Substream Sensitive SubOut
     */
    [InPort("NAME")]
    [InPort("IN")]
    public class SubOutSS : Component
    {
        /* *
         * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
         * distribute, or make derivative works under the terms of the Clarified Artistic License, 
         * based on the Everything Development Company's Artistic License.  A document describing 
         * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
         * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
         * */
        IInputPort _inport, _nameport;
        OutputPort _outport;


        public override void Execute()
        {
            Packet np = _nameport.Receive();
            if (np == null)
            {
                return;
            }
            _nameport.Close();
            string pname = np.Content as string;
            Drop(np);

            _outport = (_mother._outputPorts)[pname] as OutputPort;
            _mother.Trace(Name + ": Accessing output port: " + _outport.Name);
            _outport.SetSender(this);
            Packet p;
            p = Create(Packet.Types.Open, "");
            _outport.Send(p);
            while ((p = _inport.Receive()) != null)
            {
                _outport.Send(p);

            }
            p = Create(Packet.Types.Close, "");
            _outport.Send(p);

            //   		outport.close();
            _mother.Trace(Name + ": Releasing output port: " + _outport.Name);
            _outport = null;
        }
        //public override Object[] Introspect()
        //{
        //    return new Object[] {
	   	//	"handles one output stream for subnet",
	   //		"NAME", "input", Type.GetType("System.String"),
		//		"name of higher level output port",
		//	"IN", "input", Type.GetType("System.Object"),
		//		"input (from subnet) to external port"};
       // }
        public override void OpenPorts()
        {

            _nameport = OpenInput("NAME");
            _inport = OpenInput("IN");
        }
    }


}
