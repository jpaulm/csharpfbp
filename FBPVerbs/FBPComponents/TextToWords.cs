using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
//using System.Text.RegularExpressions;
using FBPLib;


namespace Components
{
    [InPort("IN")] 
    [OutPort("OUTN")]  
    [OutPort("OUT")]
    [ComponentDescription("Read a chunk of text from IN, write words to OUT, rejects to OUTN. Set attributes: Text=input text, Offset=word offset in text")]
    public class TextToWords : Component
    {
        // Thanks to David Bennett

        internal static string _copyright =
        "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
        "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
        "based on the Everything Development Company's Artistic License.  A document describing " +
        "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
        "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        IInputPort _inp;
        OutputPort _outp;
        OutputPort _outpn;
        /*
        public override object[] Introspect()
        {
            return new object[] { 
                "Read a chunk of text from IN, write words to OUT, rejects to OUTN.",
                "Set attributes: Text=input text, Offset=word offset in text",
                "Non-looper. 1->N"
            };
        }
        */
        public override void OpenPorts()
        {
            _inp = OpenInput("IN");
            _outp = OpenOutput("OUT");
            _outpn = OpenOutput("OUTN");
        }

        public override void Execute()
        {
            Packet p = _inp.Receive();
            string text = p.Content.ToString();
            StringBuilder sb = new StringBuilder();
            bool inword = false;
            int offs = 0;
            int offset = 0;
            foreach (char c in text + ' ')
            {
                if (Char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                    if (!inword)
                    {
                        inword = true;
                        offset = offs;
                    }
                }
                else if (inword)
                {
                    Packet pp = Create(sb.ToString());
                    pp.Attributes.Add("Text", text);
                    pp.Attributes.Add("Offset", offset);
                    
                    if (sb.Length >= 4)
                        _outp.Send(pp);
                    else
                        _outpn.Send(pp);
                    sb.Length = 0;
                    inword = false;
                }
                ++offs;
            }
            Drop(p);   
        }
        
    }
}
