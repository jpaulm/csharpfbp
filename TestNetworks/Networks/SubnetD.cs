using System;
using System.Collections.Generic;
using System.Text;
using Components;
using FBPLib;

namespace TestNetworks.Networks
{
    [InPort("IN")]
    class SubnetD : SubNet
    {
        public override void Define()
        {
            _deadlockTest = true;
    Component("IN", typeof(SubIn));
    Component("discard", typeof(Discard));
    Connect("IN.OUT", "discard.IN");
    Initialize("IN", "IN.NAME");
        }
    }
}
