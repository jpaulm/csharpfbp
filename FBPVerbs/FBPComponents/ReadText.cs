using System;
using System.IO;
using FBPLib;

namespace Components
{
    /** Component to read data from a file or other input medium, generating
* a stream of packets.   The data source is specified as a Stream Reader
* via an InitializationConnection.
*/
    [InPort("SOURCE")]
    [InPort("CONFIG")]
    [OutPort("OUT")]
    [ComponentDescription("Reads input from a character stream and outputs it line-by-line")]
    public class ReadText : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";



        OutputPort _outport;
        IInputPort _source;
        IInputPort _cfgp;
        double _timeout = 1.5;   // 1.5 secs

        public override void Execute() /* throws Throwable*/ {

            Packet p = _cfgp.Receive();
            if (p != null)
            {
                string param = p.Content.ToString();
                _timeout = Double.Parse(param);
                Drop(p);
                _cfgp.Close();
            }

            Packet rp = _source.Receive();
            _source.Close();

            if (rp == null)
                return;
            Object o = rp.Content;
            StreamReader sr = null;
            if (o is String)
            {
                String st = (String)o;
                sr = new StreamReader(st);
            }
            else
            {
                Stream str = (Stream)o;
                sr = new StreamReader(str);
            }
            Drop(rp);

            string s;
            int no = 0;
            while ((s = ReadLine(sr)) != null)
            {
                no++;
                p = Create(s);
                _outport.Send(p);
            }

            sr.Close();
            Console.Out.WriteLine("Number of lines read: {0}", no);
        }
        /*
        public override Object[] Introspect()
        {
            return new Object[] {
		"reads input from a Reader " +
		"(a character stream) and outputs it line-by-line",
		"OUT", "output", Type.GetType("System.String"),
			"lines read",
        // SOURCE may either be a Stream or a String, in
        // which case it is a file name 
		"SOURCE", "parameter", Type.GetType("System.Stream"),
			"Reader to read lines from",
        "CONFIG", "parameter", null,
			"timeout interval"};
        }
        */
        public override void OpenPorts()
        {

            _outport = OpenOutput("OUT");
           // _outport.SetType(Type.GetType("System.String"));

            _source = OpenInput("SOURCE");
           // _source.SetType(Type.GetType("System.Stream"));
            _cfgp = OpenInput("CONFIG");

        }
        string ReadLine(StreamReader sr)
        {
            string s;
            //using (TimeoutHandler t = new TimeoutHandler(_timeout, this))
            //{
            LongWaitStart(_timeout);
            //Thread.Sleep(1000);   // testing only                                                
            s = sr.ReadLine();
            //Console.Out.WriteLine("Record read");
            //}
            LongWaitEnd();
            return s;
        }
    }

}
