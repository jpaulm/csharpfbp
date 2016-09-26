using System;
using System.Collections.Generic;
using System.Text;
using FBPLib;
using Components;
using System.IO;

namespace TestNetworks.Networks
{
    /** This is really the front end of an Update app - instead of routing the merged stream 
 * to a processing component, we just display it.
 * 
 * @author HP_Administrator
 *
 */
    public class Update : Network
    {
        /* *
               * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
               * distribute, or make derivative works under the terms of the Clarified Artistic License, 
               * based on the Everything Development Company's Artistic License.  A document describing 
               * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
               * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
               * */
        public override void Define() /*throws Throwable*/ {
            Component("@Read@ Master (1)", typeof(ReadText));
            Component("@Read@ Details (2)", typeof(ReadText));
            Component("@@Collate (3)", typeof(Collate));
            Component("@@Display (4)", typeof(WriteToConsole));
            Connect("@Read@ Master (1).OUT", "@@Collate (3).IN[0]");
            Connect("@Read@ Details (2).OUT", "@@Collate (3).IN[1]");
            Connect("@@Collate (3).OUT", "@@Display (4).IN");
            Initialize(@"..\..\mfile", "@Read@ Master (1).SOURCE");
            Initialize(@"..\..\dfile", "@Read@ Details (2).SOURCE");
            //Stream st = Console.OpenStandardOutput();
            //Initialize(st, "@@Display (4).DESTINATION");

            Initialize("3,2,5", "@@Collate (3).CTLFIELDS");

        }
       

    }
}
