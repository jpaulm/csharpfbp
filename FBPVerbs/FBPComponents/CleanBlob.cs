using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FBPLib;


namespace Components
{
    [InPort("IN")] 
    [OutPort("OUT")]  
    [ComponentDescription("Clean up blobs")]
    public class CleanBlob : Component
    {

        internal static string _copyright =
           "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
           "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
           "based on the Everything Development Company's Artistic License.  A document describing " +
           "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
           "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;
        OutputPort _outport;
        Regex _rgx = new Regex(@"\P{L}");  //any character NOT a Unicode letter
        Regex _rgx2 = new Regex(@"\s+");   //at least one blank


        // make it a non-looper 
        public override void Execute() /* throws Throwable */  {

            Packet p = _inport.Receive();
            string text = p.Content as string;
            text = _rgx.Replace(text, " ");   // special characters -> spaces
            text = _rgx2.Replace(text, " ");  // collapse multiple spaces
            text = text.Trim();  // remove leading and trailing spaces
            p.Content = text;
            _outport.Send(p);


        }
      
        public override void OpenPorts()
        {
            _inport = OpenInput("IN");
            //_inport.SetType(Type.GetType("System.Object"));
            _outport = OpenOutput("OUT");
        }
    }
}

