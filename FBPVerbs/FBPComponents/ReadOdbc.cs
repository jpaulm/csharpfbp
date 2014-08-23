using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using FBPLib;

namespace Components
{
    [ComponentDescription("Execute query and write records to OUT")]
    [InPort("CONFIG", description="DSN, Query, Table, timeout_value")] 
    [OutPort("OUT")]
    
    public class ReadOdbc : Component
    {
        /* Thanks to David Bennett, Melbourne, Australia */
        internal static string _copyright =
        "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
        "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
        "based on the Everything Development Company's Artistic License.  A document describing " +
        "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
        "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";

        OutputPort _outp;
        IInputPort _cfgp;
        string _dsn;
        string _query;
        string _table;
        double _timeout;     
        /*
        public override object[] Introspect()
        {
            return new object[] {
                "Execute query and write records to OUT",
                "CONFIG DSN=string; Query=string; Table=string",
                "Generator"
            };
        }
        */
        public override void OpenPorts()
        {
            _cfgp = OpenInput("CONFIG");
            _outp = OpenOutput("OUT");
        }

        public override void Execute()
        {
            Packet p = _cfgp.Receive();
            string parms = p.Content as string;
            Drop(p);
            _cfgp.Close();

            string[] parmArray = parms.Split(',');
            _dsn = parmArray[0];
            _query = parmArray[1];
            _table = parmArray[2];
            _timeout = Double.Parse(parmArray[3]);

            OdbcConnection conn = new OdbcConnection(_dsn);
            OdbcCommand cmd = new OdbcCommand(_query, conn);
            OdbcDataReader rdr = null;
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                FlowError.Complain("cannot open connection");
            }
            try
            {
                rdr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                FlowError.Complain("cannot execute query");
            }
            List<string> columns = new List<string>();
            for (int i = 0; i < rdr.FieldCount; ++i)
                columns.Add(rdr.GetName(i));
            int serialno = 0;
            while (ReadOdbcRow(rdr))
            {
                Record rec = new Record(_table, columns.ToArray());
                for (int i = 0; i < rdr.FieldCount; ++i)
                    rec[i] = rdr[i];
                p = Create(rec);
                p.Attributes["SerialNo"] = ++serialno;
                _outp.Send(p);
            }
            // _outp.Close( ); --not needed  
        }
        bool ReadOdbcRow(OdbcDataReader rdr)
        {
            //using (TimeoutHandler t = new TimeoutHandler(_timeout, this))  
           // {
            LongWaitStart(_timeout);
                //Thread.Sleep(10000);   // testing only                                               
                bool res = rdr.Read();
                LongWaitEnd();
                return res;
            //}
        }
        
    }
}
