using System;
using System.Collections.Generic;
using System.Text;

namespace FBPLib
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct,AllowMultiple=true)]
    public class OutPort : Attribute
    {
        /* *
            * Copyright 2007, ..., 2011, J. Paul Morrison.  At your option, you may copy, 
            * distribute, or make derivative works under the terms of the Clarified Artistic License, 
            * based on the Everything Development Company's Artistic License.  A document describing 
            * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
            * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
            * */

        public OutPort(string value)  // value is positional parameter; rest are keyword
        {
            this.value = value;
        }

        public string value;
        public string[] valueList;
        public bool arrayPort;
        public bool fixedSize;
        public int setDimension;
        public bool optional;
        public Type type = typeof(Object);
    }
}