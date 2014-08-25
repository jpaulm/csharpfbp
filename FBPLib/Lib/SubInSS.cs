using System;
using System.Collections.Generic;
using System.Text;

namespace FBPLib
{
    /**
  * Look after input to subnet - added for subnet support
  * Substream-Sensitive SubIn
     * Enhanced as per Sven Steinseifer's suggestion
  */
    [InPort("NAME")]
    [OutPort("OUT")]
    public class SubInSS : Component
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
            int level = 0;
            // I think this works!
            Component oldReceiver = _inport.Receiver;
            if (_inport is InitializationConnection)
                FlowError.Complain("SubinSS cannot support IIP - use Subin");
            Connection cnxt = _inport as Connection;
            cnxt.SetReceiver(this);
            while ((p = cnxt.Receive()) != null)
            {
                p.Owner = this;
                if (p.Type == Packet.Types.Open)
                {
                    if (level == 0)
                    {
                        Drop(p);
                        _network.Trace("{0}: Open bracket detected", Name);
                    }
                    else
                        _outport.Send(p);
                    level++;
                    
                }
                else if (p.Type == Packet.Types.Close)
                {
                    level--;
                    if (level == 0)
                    {
                        Drop(p);
                        _network.Trace("{0}: Close bracket detected", Name);
                    }
                    else
                        _outport.Send(p);
                    
                    break;
                }
                else
                    _outport.Send(p);
            }


            // inport.close();
            _mother.Trace(Name + ": Releasing input port: " + _inport.Name);
            cnxt.SetReceiver(oldReceiver);

            // inport = null;
        }

        //public override Object[] Introspect()
        //{
        //    return new Object[] { "handles one input substream for subnet", "NAME",
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
