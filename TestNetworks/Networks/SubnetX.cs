using System;
using System.Collections.Generic;
using System.Text;
using Components;
using FBPLib;

namespace TestNetworks.Networks
{
    [OutPort("OUT")] 
	[InPort("IN")]
    class SubnetX : SubNet
    {
        public override void Define()
        {
            Component("SUBIN", typeof(SubInSS));
            Component("SUBOUT", typeof(SubOutSS));
            Component("Pass", typeof(Passthru));

            Initialize("IN", Component("SUBIN"), Port("NAME"));
            Connect(Component("SUBIN"), Port("OUT"), Component("Pass"), Port("IN"));
            Connect(Component("Pass"), Port("OUT"), Component("SUBOUT"), Port("IN"));
            Initialize("OUT", Component("SUBOUT"), Port("NAME"));
        }
    }
} 
