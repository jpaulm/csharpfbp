using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FBPLib
{
    public class Port
    {
        /* 
 *  This class ensures that Network connect statements use "real" port names, 
 *  rather than arbitrary character strings 
 */

        public string _name;

        public int _index; // -1 if array port

        public string _displayName = null;

        public Port(string n, int i)
        {
            _name = n;
            _index = i;
            if (_index == -1)
            {
                Regex re = new Regex(@"^(\w+)\[(\d+)\]$");  // test for square bracket
                Match m = re.Match(_name);
                if (m.Success)
                {
                    _name = m.Groups[1].Value;
                    _index = Int32.Parse(m.Groups[2].Value);
                }
            }
            if (_index != -1)  // index may have changed!
                _displayName = String.Format("{0}[{1}]", _name, _index);
            else 
                _displayName = String.Format("{0}", _name);
        }
    }
}

