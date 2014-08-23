using System;
using System.IO;
using FBPLib;
using System.Net.Sockets;
using System.Net;



namespace Components
{
    /** Component to read data from a socket, generating
* a stream of packets.   Server side.
*/
    //[InPort("SOURCE")]
    [InPort("PORT")]
    [OutPort("OUT")]
    [ComponentDescription("Reads input from a socket and outputs it line-by-line")]
    public class ReadFromSocket : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";



        OutputPort _outport;
        //IInputPort _source;
        IInputPort _port;
        double _timeout = 30.0;   // 30.0 secs
        System.IO.StreamReader streamReader = null;
        System.IO.StreamWriter streamWriter = null;
        NetworkStream networkStream = null;


        public override void Execute() /* throws Throwable*/ {
            int port = 0;
            string theString;
            int cyclic_count = 0;
            Packet p = _port.Receive();
            if (p != null)
            {
                string portno = p.Content.ToString();
                port = Int32.Parse(portno);
                Drop(p);
                _port.Close();
            }

            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            TcpListener tcpServerListener = new TcpListener(localAddr, port);
            tcpServerListener.Start();
            TcpClient client = tcpServerListener.AcceptTcpClient();

            try
            {
                if (client.Connected)
                {
                    Console.Out.WriteLine("RS Client connected");
                    //open network stream on accepted socket

                    networkStream = client.GetStream();
                    streamWriter = new StreamWriter(networkStream);
                    streamReader = new StreamReader(networkStream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return;
            }

            LongWaitStart(_timeout);
            theString = streamReader.ReadLine();
            while (!(theString.Equals("Closedown")))
            {
                LongWaitEnd();
                string s = theString.Substring(0, 4);
                theString = theString.Substring(5);
                int i = Int32.Parse(s);
                if (i != cyclic_count)
                {
                    Console.Error.WriteLine("Sequence error: " + this.Name);
                    break;
                }
                //Console.Out.WriteLine("RS read: " + theString);
                ///* Experimental
                if (cyclic_count % 20 == 0)
                {
                    streamWriter.WriteLine(theString);
                    //Console.Out.WriteLine("RS ack: " + theString);
                    streamWriter.Flush();
                }
                //*/

                p = Create(theString);
                _outport.Send(p);
                LongWaitStart(_timeout);
                cyclic_count = (cyclic_count + 1) % 10000;
                theString = streamReader.ReadLine();
            }
            LongWaitEnd();
            streamReader.Close();
            networkStream.Close();
            streamWriter.Close();

            client.Close();
            Console.WriteLine(this.Name + " Exiting...");
            tcpServerListener.Stop();
        }

        public override void OpenPorts()
        {

            _outport = OpenOutput("OUT");
            // _outport.SetType(Type.GetType("System.String"));

            //_source = OpenInput("SOURCE");
            // _source.SetType(Type.GetType("System.Stream"));
            _port = OpenInput("PORT");

        }

    }

}
