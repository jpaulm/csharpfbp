namespace FBPLib
{
    using System;


    /// <summary> Instances of this class are thrown whenever a programming error
    /// in a flow Network is detected.  Nobody is expected to catch these,
    /// because they are considered indications of design errors, even though
    /// detected only at run-time.
    /// *
    /// </summary>

    public class FlowError : System.ApplicationException
    {
        /* *
           * Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, 
           * distribute, or make derivative works under the terms of the Clarified Artistic License, 
           * based on the Everything Development Company's Artistic License.  A document describing 
           * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
           * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
           * */
        /// <summary> Constructs a new FlowError with a useful (but non-localized)
        /// message as its text.  FlowErrors without texts are not allowed, as
        /// these are considered bad practice.
        /// </summary>
        /// <param name="text">a description of the error
        /// *
        /// </param>
        internal FlowError(string text)
            : base(text)
        {
        }
        /// <summary> A convenience method which constructs a new FlowError and
        /// throws it at once, typically never returning.
        /// </summary>
        /// <param name="text">a description of the error
        /// *
        /// </param>
        public static void Complain(string text)
        {
            Console.Out.WriteLine(text);
            throw new FlowError(text);
        }
        public static void Assert(bool check, string message)
        {
            if (!check)
                Complain(message);
        }

        public static void Assert(bool check, string format, params object[] args)
        {
            if (!check)
                Complain(String.Format(format, args));
        }
        public static void AssertNotNull(object arg, string argname)
        {
            if (arg == null)
                Complain(argname + " is null");
        }
    }
}
