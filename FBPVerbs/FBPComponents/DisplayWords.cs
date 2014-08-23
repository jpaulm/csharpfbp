using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FBPLib;


namespace Components
{
    [InPort("IN")]
    [OutPort("OUT")]
    [OutPort("OUTD")]
    [ComponentDescription("Build records from incoming word packets")]
    public class DisplayWords : Component
    {
        internal static string _copyright =
        "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
        "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
        "based on the Everything Development Company's Artistic License.  A document describing " +
        "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
        "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        IInputPort _inp;
        OutputPort _outp;
        OutputPort _outdp;
        int _reclen = 30;
        /*
        public override object[] Introspect()
        {
            return new object[] { 
                "Read a word from IN, write a concordance line to OUT.",
                "Uses attributes: Text and Offset",
                "Non-looper. 1->1"
            };
        }
        */
        public override void OpenPorts()
        {
            _inp = OpenInput("IN");
            _outp = OpenOutput("OUT");
            _outdp = OpenOutput("OUTD");
        }


        public override void Execute()
        {
            Packet p;
            string record = "";
            while ((p = _inp.Receive()) != null)
            {
                string word = p.Content.ToString();

                if (record.Length >= _reclen)
                {
                    _outdp.Send(Create(record));
                    record = "";
                }
                record += word + " ";
                _outp.Send(p);
            }
            if (record.Length > 0)
                _outdp.Send(Create(record));
        }
    }
}

