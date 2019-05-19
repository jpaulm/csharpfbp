using System;
using System.Diagnostics;
using FBPLib;


namespace Components
{
    /** Component to run an EXE
*/
    
    [ComponentDescription("Run an .exe file")]
    public class RunExe : Component
    {

        internal static string _copyright =
        "Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, " +
        "distribute, or make derivative works under the terms of the Clarified Artistic License, " +
        "based on the Everything Development Company's Artistic License.  A document describing " +
        "this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. " +
        "THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.";



        

        public override void Execute() {

            // Thanks to Lars Behrmann - http://bytes.com/groups/net-c/279036-running-exe

            //ProcessStartInfo startInfo = new ProcessStartInfo("IExplore.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo("DRAWFLOW.EXE");
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.WorkingDirectory = @"C:\Documents and Settings\HP_Administrator\My Documents\Business\C++DrawFBP";

            //Process.Start(startInfo);
            //startInfo.Arguments = "www.jpaulmorrison.com";
            Process.Start(startInfo);

        }
        /*
        public override Object[] Introspect()
        {
            return new Object[] {
		"concatenates input streams at array port IN and sends them to port OUT",
		"IN", "input", Type.GetType("Object"),
			"input stream",
		"OUT", "output", Type.GetType("Object"),
			"output stream"};
        }
        */
        public override void OpenPorts()
        {

            

        }
    }

}
