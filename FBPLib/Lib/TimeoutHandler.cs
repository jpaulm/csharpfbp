using System;
using System.Collections.Generic;
using System.Text;

namespace FBPLib
{
    public class TimeoutHandler
    {
        /* *
            * Copyright 2007, ..., 2011, J. Paul Morrison.  At your option, you may copy, 
            * distribute, or make derivative works under the terms of the Clarified Artistic License, 
            * based on the Everything Development Company's Artistic License.  A document describing 
            * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
            * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
            * */
        bool disposed = false;
        internal int _dur;            // duration of TimeoutHandler
        internal Component _comp;


        public TimeoutHandler(double dur, Component comp)
        {
            lock (comp._network._timeouts)
            {
                comp.Timeout = this;
                comp._network._timeouts.Add(comp, this);
                double ms = dur * 1000.0 + 500.0;
                _dur = (int)ms;  //convert to msecs

                comp.Status = Component.States.LongWait;
                _comp = comp;
            }
        }
        public void Dispose()
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                lock (_comp._network._timeouts)
                {
                    _comp.Status = Component.States.Active;
                    _comp._network._timeouts.Remove(_comp);
                    _comp.Timeout = null;
                }

                disposed = true;
            }
        }


        internal void Decrement(int freq)
        {
            _dur -= freq;  // reduce by frequency, in msecs
            if (_dur < 0)
                FlowError.Complain("Component " + _comp.Name + " timed out");
        }
    }

}


