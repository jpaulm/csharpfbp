using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;

namespace Components
{

    /*    
    *   Component to collate two or more streams of packets, based 
    *   on a series of control fields;  control fields are assumed 
     * to be contiguous, and to start at offset 0 in each packet
     * Groups of packets are grouped together using open and close brackets -
     * i.e. if there are 3 control fields (called major, intermediate
     * and minor), minor groupings will be surrounded by single brackets,
     * intermediate by double brackets, and major by triple brackets.
     * -- Written by PM
    */
    [InPort("CTLFIELDS")]
    [InPort("IN", arrayPort = true)]
    [OutPort("OUT")]
    [ComponentDescription("Collates input streams at array port IN and sends them to port OUT")]
    public class Collate : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort[] _inportArray;
        OutputPort _outport;
        IInputPort _cfgp;

        int[] _fldArray;
        string prev = null;
        string hold = null;
        int low;
        int parmct;
        Packet[] pArray;
        StringComparer ordCmp = StringComparer.Ordinal;

        public override void Execute()
        {
            Packet p = _cfgp.Receive();
            string s = p.Content as string;
            string[] stArray = s.Split(',');
            _fldArray = new int[stArray.Length];
            for (int i = 0; i < stArray.Length; i++)
            {
                stArray[i].Trim();
                _fldArray[i] = Int32.Parse(stArray[i]);
            }
            
            Drop(p);
            _cfgp.Close();

            parmct = _fldArray.Length;
            int totlen = 0;
            for (int i = 0; i < parmct; i++)
                totlen += _fldArray[i];

            for (int i = 0; i < parmct; i++)
            {
                Packet p2 = Create(Packet.Types.Open, " ");
                _outport.Send(p2);
            }
            int no = _inportArray.Length;
            int count = no;   // count of not drained input ports
            pArray = new Packet[no];
            
            for (int i = 0; i < no; i++)
            {
                p = _inportArray[i].Receive();
                if (p == null)
                {
                    pArray[i] = null;
                    --count;
                }
                else pArray[i] = p;

            }

            while (true)
            {
                hold = "\uffff";
                low = 0;

                for (int i = 0; i < no; i++)
                {
                    if (pArray[i] != null)
                    {

                        string value = (string)pArray[i].Content;
                        value = value.Substring(0, totlen);

                        if (ordCmp.Compare(value,hold) < 0)
                        {
                            hold = value;
                            low = i;
                        }
                    }


                }
                SendOutput(low);
                pArray[low] = null;
                p = _inportArray[low].Receive();
                if (p == null)
                    count--;
                else pArray[low] = p;
                if (count == 0) break;

            }
            for (int i = 0; i < parmct; i++)
            {
                Packet p2 = Create(Packet.Types.Close, " ");
                _outport.Send(p2);
            }
        }
        void SendOutput(int x)
        {
            if (prev != null)
            {
                //if (hold.compareTo(prev) != 0) {
                int level = FindLevel(hold, prev);
                for (int i = 0; i < level; i++)
                {
                    Packet p2 = Create(Packet.Types.Close, " ");
                    _outport.Send(p2);
                }
                for (int i = 0; i < level; i++)
                {
                    Packet p2 = Create(Packet.Types.Open, " ");
                    _outport.Send(p2);
                }

            }
            _outport.Send(pArray[x]);
            prev = hold;
        }

        int FindLevel(string s, string t)
        {
            int j = 0;
            for (int i = 0; i < parmct; i++)
            {
                string h1 = hold.Substring(j, _fldArray[i]);
                string p1 = prev.Substring(j, _fldArray[i]);
                if (ordCmp.Compare(h1,p1) != 0)
                    return parmct - i;
                j += _fldArray[i];
            }
            return 0;
        }
     
        public override void OpenPorts()
        {

            _inportArray = OpenInputArray("IN");
            _outport = OpenOutput("OUT");
            _cfgp = OpenInput("CTLFIELDS");

        }
        
    }
}

