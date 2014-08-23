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
    [OutPort("OUT")]
    [ComponentDescription("Read a word from IN, write a concordance line to OUT")]
    public class FormatConcord : Component
    {
        internal static string _copyright =
        "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
        "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
        "based on the Everything Development Company's Artistic License.  A document describing " +
        "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
        "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        IInputPort _inp;
        OutputPort _outp;
        IInputPort _cfgp;
        int _linlen = -1;
        int _wordoff = -1;


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
            _cfgp = OpenInput("CONFIG");
            _outp = OpenOutput("OUT");
        }


        public override void Execute()
        {
            Packet p;
            if (null != (p = _cfgp.Receive()))
            {
                string param = p.Content.ToString();
                string[] words = param.Split(',');
                _wordoff = Int32.Parse(words[0]);
                _linlen = Int32.Parse(words[1]);
                Drop(p);
                _cfgp.Close();
            }

            p = _inp.Receive();
            string word = p.Content.ToString();
            string text = p.Attributes["Text"] as string;

            text = new string(' ', _linlen) + text + new string(' ', _linlen);
            int offset = (int)p.Attributes["Offset"];
            int i = offset + _linlen - _wordoff;
            int j = text.Length - i;
            j = j < _linlen ? j : _linlen;
            string line = text.Substring(i, j);
            line = line.Substring(0, _wordoff) + "*" + line.Substring(_wordoff);
            _outp.Send(Create(line));
            Drop(p);

        }

    }
}
