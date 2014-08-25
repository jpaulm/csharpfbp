using System;
using System.IO;
using FBPLib;
using System.Threading;


namespace Components
{
    /** Component to write data to a file or other output medium, using
* a stream of packets.   The data target is specified as a Stream Writer
* via an InitializationConnection.
* It is specified as "must run" so that the output file will be cleared
* even if no data packets are input.
*/
    [MustRun]
    [InPort("DESTINATION")]
    [InPort("FLUSH", optional = true)]
    [InPort("CONFIG", optional = true)]
    [InPort("IN")]
    [OutPort("OUT", optional = true)]
    [ComponentDescription("Write text to destination")]
    public class WriteText : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;
        IInputPort _destination;
        IInputPort _flush;
        IInputPort _cfgp;
        OutputPort _outport;
        static string _linesep = System.Environment.NewLine;
        double _timeout = 2.0;   // 2 secs

        public override void OpenPorts()
        {

            _inport = OpenInput("IN");
            // _inport.SetType(Type.GetType("System.String"));

            _destination = OpenInput("DESTINATION");
            //  _destination.SetType(Type.GetType("Stream"));
            _flush = OpenInput("FLUSH");
            _cfgp = OpenInput("CONFIG");
            _outport = OpenOutput("OUT");


        }

        public override void Execute() /*throws Throwable*/ {

            Packet wp = _destination.Receive();
            if (wp == null)
                FlowError.Complain("Destination not specified: " + Name);
            _destination.Close();

            Packet fp = _flush.Receive();
            if (fp != null) Drop(fp);
            _flush.Close();

            Packet p = _cfgp.Receive();
            if (p != null)
            {
                string param = p.Content.ToString();
                _timeout = Double.Parse(param);
                Drop(p);
                _cfgp.Close();
            }

            Object dest = wp.Content;
            TextWriter tw;
            if (dest is TextWriter)
                tw = dest as TextWriter;
            else if (dest is string)
                tw = new StreamWriter(dest as string);
            else if (dest is Stream)
                tw = new StreamWriter(dest as Stream);
            else
                tw = new StringWriter();

            Drop(wp);

            while ((p = _inport.Receive()) != null)
            {
                //using (TimeoutHandler t = new TimeoutHandler(_timeout, this))
                //{
                LongWaitStart(_timeout);
                //Thread.Sleep(3000);   // testing only
                if (p.Type == Packet.Types.Open)
                    tw.WriteLine("==> Open Bracket");
                else
                    if (p.Type == Packet.Types.Close)
                        tw.WriteLine("==> Close Bracket");
                    else
                        if (p.Content == null)
                            tw.WriteLine("null");
                        else
                            tw.WriteLine(p.Content);
                //}
                LongWaitEnd();

                // sw.Write(linesep);
                if (fp != null) {
                    string s = (string) fp.Content;
                    if (!(s.Equals("-")))
                    tw.Flush(); 
                }
                if (_outport.IsConnected())
                    _outport.Send(p);
                else
                    Drop(p);
            }
            tw.Close();
        }
        /*
        public override System.Object[] Introspect()
        {

            return new Object[] {
        "transmits its input to a Writer as lines, " +
		"optionally flushing the Writer after each line",
		"IN", "input", Type.GetType("System.String"),
			"lines to be written",
        // DESTINATION may either be a Stream or a String, in
        // which case it is a file name 
		"DESTINATION", "parameter", Type.GetType("Stream"),
			"Writer to write lines to",
		"FLUSH", "parameter", null,
			"flush Writer after every line if present",
        "CONFIG", "parameter", null,
			"timeout interval"};
        }
        */
    }

}
