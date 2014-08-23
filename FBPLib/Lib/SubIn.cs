using System;
using System.Collections.Generic;
using System.Text;

namespace FBPLib 
{
    /**
  * Look after input to subnet - added for subnet support
  */
    [InPort("NAME")]
    [OutPort("OUT")]
    public class SubIn : Component
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

            _inport = (_mother._inputPorts)[pname] as IInputPort;
            _mother.Trace(Name + ": Accessing input port: " + _inport.Name);
            Packet p;
            // I think this works!
            Component oldReceiver = _inport.Receiver;
            if (_inport is InitializationConnection)
            {
                InitializationConnection iico = (InitializationConnection)_inport;
                InitializationConnection iic = new InitializationConnection(
                        iico.Content, this);
                iic.Name = iico.Name;
                // iic.network = iico.network;

                p = iic.Receive();
                p.Owner = this;
                _outport.Send(p);
                iic.Close();
            }
            else
            {
                Connection cnxt = _inport as Connection;
                cnxt.SetReceiver(this);
                while ((p = cnxt.Receive()) != null)
                {
                    p.Owner = this;
                    _outport.Send(p);
                }
               // cnxt.setReceiver(oldReceiver); // moved down, as per JavaFBP
            }

            // inport.close();

            _mother.Trace(Name + ": Releasing input port: " + _inport.Name);
            _inport.SetReceiver(oldReceiver);
            // inport = null;
        }

        //public override Object[] Introspect()
        //{
        //    return new Object[] { "handles one input stream for subnet", "NAME",
		//		"input", Type.GetType("System.String"), "name of higher level input port",
		//		"OUT", "output", Type.GetType("System.Object"),
		//		"output from external port (into subnet)" };
       // }

        public override void OpenPorts()
        {

            _nameport = OpenInput("NAME");
            _outport = OpenOutput("OUT");
        }
    }

}
