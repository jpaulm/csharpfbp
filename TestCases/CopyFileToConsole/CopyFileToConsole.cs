using System;
using FBPLib;
using Components;


namespace CopyFileToConsole
{

    public class CopyFileToConsole : Network
    {

        /* *
                * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
                * distribute, or make derivative works under the terms of the Clarified Artistic License, 
                * based on the Everything Development Company's Artistic License.  A document describing 
                * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
                * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
                * */
        public override void Define() /* throws Throwable */
        {
            Connect(Component("Read", typeof(ReadText)),
                Port("OUT"),
                Component("Write", typeof(WriteToConsole)),
                Port("IN"));
            Object d = (Object)@"..\..\Resources\fake_cyrillic.txt";
            Initialize(d,
                Component("Read"),
                Port("SOURCE"));

        }
        internal static void Main(String[] argv)
        {
            new CopyFileToConsole().Go();
            Console.Read();
        }
    }

}
