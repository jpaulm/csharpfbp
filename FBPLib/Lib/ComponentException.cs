using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBPLib
{
    public class ComponentException:System.ApplicationException
    {
        public ComponentException(string s)
            :base(s)
        {
            
        }
    }
}
