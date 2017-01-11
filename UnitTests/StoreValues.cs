using System;
using FBPLib;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTests
{
    /** 
    * Component to compare the input of the component with an expected value.
    */

    [InPort("IN")]
    [OutPort("OUT", optional = true)]
    [ComponentDescription("Store values of all packets received")]
    public class StoreValues : Component
    {
        internal static string _copyright =
                "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
                "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
                "based on the Everything Development Company's Artistic License.  A document describing " +
                "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
                "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";


        IInputPort _inport;
        List<object> _values = new List<object>();

        public IReadOnlyList<object> Values => _values;

        public override void OpenPorts()
        {
            _inport = OpenInput("IN");
        }

        public override void Execute()
        {
            Packet p;
            while ((p = _inport.Receive()) != null)
            {
                if (p.Type != Packet.Types.Open && p.Type != Packet.Types.Close)
                {
                    _values.Add(p.Content);
                }

                Drop(p);
            }
        }
    }
}
