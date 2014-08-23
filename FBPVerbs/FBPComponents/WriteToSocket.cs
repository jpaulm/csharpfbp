using System;
using System.IO;
using FBPLib;
//using System.Threading;
using System.Net.Sockets;
using System.Net;



namespace Components
{
    /** Component to write data to a socket, using
* a stream of packets.   Client side.
* It is specified as "must run".  
*/
    [MustRun]
    [InPort("PORT")]
    [InPort("IN")]
    [OutPort("OUT", optional = true)]
    [ComponentDescription("Write text to socket")]
    public class WriteToSocket : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;
        IInputPort _port;
        OutputPort _outport;

        double _timeout = 10.0;   // 10 secs

        public override void OpenPorts()
        {

            _inport = OpenInput("IN");
            // _inport.SetType(Type.GetType("System.String"));

            _port = OpenInput("PORT");
            //  _destination.SetType(Type.GetType("Stream"));

            _outport = OpenOutput("OUT");


        }

        public override void Execute() /*throws Throwable*/ {


            int port = 0;
            string theString;
            Packet p = _port.Receive();
            if (p != null)
            {
                string portno = p.Content.ToString();
                port = Int32.Parse(portno);
                Drop(p);
                _port.Close();
            }
            TcpClient tcpClient;
            try
            {
                tcpClient = new TcpClient("localhost", port);
            }
            catch
            {
                Console.Error.WriteLine(
                "Failed to connect to server at {0}:{1}", "localhost", port);
                return;
            }
            NetworkStream networkStream = tcpClient.GetStream();
            System.IO.StreamReader streamReader =
            new System.IO.StreamReader(networkStream);
            System.IO.StreamWriter streamWriter =
            new System.IO.StreamWriter(networkStream);

            int cyclic_count = 0;
            while ((p = _inport.Receive()) != null)
            {
               
                try
                {
                    string s = String.Format("{0:D4}", cyclic_count);
                    streamWriter.WriteLine(s + ":" + p.Content);
                    //Console.WriteLine("WS write: " + p.Content);
                    streamWriter.Flush();
                    ///* Experimental
                    //if (cyclic_count % 20 == 0)
                    //{
                        LongWaitStart(_timeout);
                        theString = streamReader.ReadLine();
                        //Console.WriteLine("WS ack: " + theString);
                        LongWaitEnd();
                   // }
                    //*/
                }
                catch
                {
                    Console.Error.WriteLine("Exception reading from Server");
                }
               
                if (_outport.IsConnected())
                    _outport.Send(p);
                else
                    Drop(p);
                cyclic_count = (cyclic_count + 1) % 10000;
            }
            streamWriter.WriteLine("Closedown");
            Console.WriteLine(this.Name + " Closing");
            streamWriter.Flush();
            networkStream.Close();
            streamReader.Close();
            streamWriter.Close();
        }

    }

}
