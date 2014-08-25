using System;
using System.Collections.Generic;
using System.Text;

namespace FBPLib
{
    /// <summary>
    /// Implements a record as a named collection of named columns
    /// </summary>
    public class Record
    {
        /* Thanks to David Bennett */
        /* *
           * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
           * distribute, or make derivative works under the terms of the Clarified Artistic License, 
           * based on the Everything Development Company's Artistic License.  A document describing 
           * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
           * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
           * */
        string _name;
        string[] _columnnames;
        object[] _columnvalues;

        //--- accessors
        public object this[int index]
        {
            get { return _columnvalues[index]; }
            set { _columnvalues[index] = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string[] ColumnNames
        {
            get { return _columnnames; }
        }
        public object[] ColumnValues
        {
            get { return _columnvalues; }
        }
        public object GetColumn(string name)
        {
            int index = Array.IndexOf(_columnnames, name);
            return (index == -1) ? null : _columnvalues[index];
        }
        // generic get with cast to type 
        // BUG: should be Convert
        public T GetColumn<T>(string name, T dflt)
        {
            int index = Array.IndexOf(_columnnames, name);
            return (index == -1) ? dflt : (T)_columnvalues[index];
        }

        public void SetColumn(string name, object value)
        {
            int index = Array.IndexOf(_columnnames, name);
            FlowError.Assert(index != -1, "unknown column");
            _columnvalues[index] = value;
        }
        public override string ToString()
        {
            List<string> vv = new List<string>();
            for (int i = 0; i < _columnnames.Length; ++i)
                vv.Add(String.Format("{0}:{1}", _columnnames[i], _columnvalues[i]));
            return String.Format("{{[{0}] {1}}}", _name, string.Join(" ", vv.ToArray()));
        }

        //--- ctors
        public Record(string name, string[] columnnames) : this(name, columnnames, new object[columnnames.Length]) { }
        public Record(string name, string[] columnnames, object[] columnvalues)
        {
            FlowError.AssertNotNull(name, "name");
            FlowError.AssertNotNull(columnnames, "column names");
            FlowError.AssertNotNull(columnvalues, "column values");
            FlowError.Assert(columnvalues.Length == columnnames.Length, "column names and values different length");
            _name = name;
            _columnnames = columnnames;
            _columnvalues = columnvalues;
        }
    }
}
