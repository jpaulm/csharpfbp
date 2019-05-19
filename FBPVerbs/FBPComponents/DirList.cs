using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FBPLib;


namespace Components
{
    [InPort("IN")] 
    [OutPort("OUT")]  
    [OutPort("OUTF")]  
    [OutPort("OUTD")]
    [ComponentDescription("Read directory path from IN, write it to OUT, files to OUTF, directories to OUTD")]
    
    public class DirList : Component
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
        OutputPort _outpf;
        OutputPort _outpd;
      
        public override void OpenPorts()
        {
            _inp = OpenInput("IN");
            _outp = OpenOutput("OUT");
            _outpd = OpenOutput("OUTD");
            _outpf = OpenOutput("OUTF");
        }

        public override void Execute()
        {
            Packet p = _inp.Receive();
            string name = p.Content.ToString();
            _outp.Send(p);
            _inp.Close();
            
            DirectoryInfo di = new DirectoryInfo(name);
            DirectoryInfo[] dirs = di.GetDirectories();
            if (dirs.Length == 0)
                FlowError.Complain("Missing directory");
            foreach (DirectoryInfo d in dirs)
                _outpd.Send(Create(d.FullName));
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo f in files)
                _outpf.Send(Create(f.FullName));
        }
        
    }
}
