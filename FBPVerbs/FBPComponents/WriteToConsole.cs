using System;
using FBPLib;


namespace Components
{
    /** 
    * Component to write data to console.
    */
          
    [InPort("IN")]
    [OutPort("OUT", optional = true)]
    [ComponentDescription("Write text to console")]

    public class WriteToConsole : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;        
        OutputPort _outport;
        static string _linesep = System.Environment.NewLine;
        double _timeout = 2.0;   // 2 secs          

        public override void OpenPorts()
        {
            _inport = OpenInput("IN");       
            _outport = OpenOutput("OUT");
        }

        public override void Execute() /*throws Throwable*/
        {
            Packet p;
            while ((p = _inport.Receive()) != null)
            {                                         
                string line = p.Content as string;
                LongWaitStart(_timeout);
                //Thread.Sleep(3000);   // testing only
                if (p.Type == Packet.Types.Open)
                    Console.Out.WriteLine("==> Open Bracket");
                else
                    if (p.Type == Packet.Types.Close)
                    Console.Out.WriteLine("==> Close Bracket");
                else
                        if (p.Content == null)
                    Console.Out.WriteLine("null");
                else
                    Console.Out.WriteLine(p.Content);
                //}
                LongWaitEnd();
                if (_outport.IsConnected())
                {
                    _outport.Send(p);
                }
                else
                {
                    Drop(p);
                }
            }
           
        }
       
    }

}
