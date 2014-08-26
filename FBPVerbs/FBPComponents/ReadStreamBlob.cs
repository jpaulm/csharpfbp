using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FBPLib;


namespace Components
{
    [InPort("IN")]
    [InPort("CONFIG", optional=true)] 
    [OutPort("OUT")]
    [ComponentDescription("Read name of input file from IN, output contents as blob to OUT")]
    public class ReadStreamBlob : Component
    {
        /* Thanks to David Bennett, Melbourne, Australia */
        internal static string _copyright =
        "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
        "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
        "based on the Everything Development Company's Artistic License.  A document describing " +
        "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
        "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        IInputPort _inp;
        OutputPort _outp;
        IInputPort _cfgp;
        double _timeout = 3.0;
        /*
        public override object[] Introspect()
        {
            return new object[] {
                "Read name of input file from IN, output contents as blob to OUT",
                "Sets attributes: Path=input path",
                "Non-looper. 1->1"
            };
        }
        */
        public override void OpenPorts()
        {
            _inp = OpenInput("IN");
            _outp = OpenOutput("OUT");
            _cfgp = OpenInput("CONFIG");
        }

        public override void Execute()
        {
            Packet p = _cfgp.Receive();
            if (p != null)
            {
                string param = p.Content.ToString();
                _timeout = Double.Parse(param);
                Drop(p);
                _cfgp.Close();
            }
            
            Packet p0 = _inp.Receive();   
            string path = p0.Content as string;     
            if (path != null)
            {
                TextReader tr = new StreamReader(path);
                string blob = ReadBlobItem(tr);
                p = Create(blob);
                p.Attributes.Add("Path", path);
                _outp.Send(p);
            }
            Drop(p0);
        }
        string ReadBlobItem(TextReader tr)
        {
            //using (TimeoutHandler t = new TimeoutHandler(_timeout, this))  //db's suggestion
            //{
            LongWaitStart(_timeout);
                //Thread.Sleep(10000);   //   testing only                                               //pm 
                string s = tr.ReadToEnd();
            //}
                LongWaitEnd();
                return s;
        }
        
    }
}
