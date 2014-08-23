using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using FBPLib;


namespace Components
{
    [InPort("IN")]
    [InPort("CONFIG")] 
    [OutPort("OUTN")]  
    [OutPort("OUT")]
    [ComponentDescription("Read from IN, compare to Regex in CONFIG, write to OUT if match, else OUTN")] 

    public class Match : Component
    {
        // Regex code contributed by David Bennett

        internal static string _copyright =
        "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
        "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
        "based on the Everything Development Company's Artistic License.  A document describing " +
        "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
        "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        IInputPort _inp;
        IInputPort _cfgp;
        OutputPort _outp;
        OutputPort _outpn;
        /*
        public override object[] Introspect()
        {
            return new object[] { 
                "Read from IN, compare to Regex in CONFIG, write to OUT if match else OUTN.",
                "Looper. N->M+~M"
            };
        }
        */
        public override void OpenPorts()
        {
            _cfgp = OpenInput("CONFIG");
            _inp = OpenInput("IN");
            _outp = OpenOutput("OUT");
            _outpn = OpenOutput("OUTN");

        }

        public override void Execute()
        {
            Regex regex = null;
            Packet p = _cfgp.Receive();
            string pattern = p.Content.ToString();
            try
            {
                regex = new Regex(pattern);
            }
            catch (Exception)
            {
                FlowError.Complain("invalid regular expression " + pattern);
            }
            Drop(p);
            _cfgp.Close();

            for (p = _inp.Receive(); p != null; p = _inp.Receive())
            {
                if (regex.IsMatch(p.Content.ToString()))
                    _outp.Send(p);
                else
                    _outpn.Send(p);
            }
        }


    }
}
