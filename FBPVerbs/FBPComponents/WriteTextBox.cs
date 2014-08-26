using System;
using System.IO;
using FBPLib;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;


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
    [InPort("CONFIG", optional = true)]
    [InPort("IN")]
    [OutPort("OUT")]
    [ComponentDescription("Write text to text box")]
    public class WriteTextBox : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;
        IInputPort _destination;

        IInputPort _cfgp;
        OutputPort _outport;
        static string _linesep = System.Environment.NewLine;
        double _timeout = 2.0;   // 2 secs
        delegate void DummyMethod();

        public override void OpenPorts()
        {

            _inport = OpenInput("IN");
            //_inport.SetType(Type.GetType("System.String"));

            _destination = OpenInput("DESTINATION");
           // _destination.SetType(Type.GetType("Stream"));

            _cfgp = OpenInput("CONFIG");
            _outport = OpenOutput("OUT");


        }

        public override void Execute() /*throws Throwable*/ {
            
            Packet wp = _destination.Receive();
            if (wp == null)
                return;
            _destination.Close();

            Packet p = _cfgp.Receive();
            if (p != null)
            {
                string param = p.Content.ToString();
                _timeout = Double.Parse(param);
                Drop(p);
                _cfgp.Close();
            }

            TextBox tb = wp.Content as TextBox;            
            Drop(wp);
            
            while ((p = _inport.Receive()) != null)
            {
                LongWaitStart(_timeout);
                //Thread.Sleep(3000);   // testing only                                                
                string line = p.Content as string;
                if (tb.Multiline)
                    line += "\r\n";
                DummyMethod dg = delegate()
                 {
                   tb.AppendText(line);
                 };
                
                tb.Invoke(dg);
                LongWaitEnd();
                _outport.Send(p);
            }
            // tw.Close();
        }
        /*
        public override System.Object[] Introspect()
        {

            return new Object[] {
        "transmits its input to a Writer as lines",
		"IN", "input", Type.GetType("System.String"),
			"lines to be written",
        // DESTINATION may either be a Stream or a String, in
        // which case it is a file name 
		"DESTINATION", "parameter", Type.GetType("Stream"),
			"Writer to write lines to",
        "CONFIG", "parameter", null,
			"timeout interval"};
        }
        */
    }

}
