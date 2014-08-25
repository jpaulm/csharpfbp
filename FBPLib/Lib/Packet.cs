namespace FBPLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>A Packet may either contain an Object, when <code> type</type> is <code> PacketTypes.Normal </code>,
    /// or a string, when <code>type</code> is not <code>PacketTypes.Normal</code>.  The latter case
    /// is used for things like open and close brackets (where the string will be the name
    /// of a group. e.g. accounts)
    /// </summary>

    public class Packet:IDisposable
    {
        /* *
          * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
          * distribute, or make derivative works under the terms of the Clarified Artistic License, 
          * based on the Everything Development Company's Artistic License.  A document describing 
          * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
          * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
          * */
        private void InitBlock()
        {
            _nullIter = new Hashtable().GetEnumerator();
            _attributes = new Dictionary<string, object>();
        }
        //--- attributes
        public Dictionary<string, object> Attributes
        {
            get { return _attributes; }

        }
        /*
        internal object GetAttribute(string name)
        {
            return _attributes.ContainsKey(name) ? _attributes[name] : null;
        }
        internal T GetAttribute<T>(string name, T dflt)
        {
            return _attributes.ContainsKey(name) ? (T)_attributes[name] : dflt;
        }
        */

        internal IEnumerable<string> Children
        {
            get
            {
                return (_chains == null) ? null : _chains.Keys;
            }
        }
        internal List<Packet> Child(string name)
        {
            return _chains[name].Members;
        }
        public virtual System.Object Content
        {
            get
            {
                if (_type == Types.Normal)
                    return _content;
                else
                    return null;
            }
            set { _content = value; }

        }
        internal virtual string Name
        {
            get
            {
                if (_type == Types.Normal)
                    return null;
                else
                    return (string)(_content);
            }

        }
        /*
        internal virtual Packet Root
        {
            get
            {
                Packet p = this;
                while (p._owner is Chain)
                {
                    p = ((Chain)(p._owner)).Head;
                }
                return p;
            }

        }
        */
        public virtual Types Type
        {
            get
            {
                return _type;
            }

        }
        internal virtual System.Object Owner
        {
            set
            {
                ClearOwner();
                _owner = value;
                if (_owner is Component)
                {
                    Component c = (Component)(_owner);
                    c._packetCount++; // count of owned packets
                }
            }

        }

        public enum Types
        { Normal, Open, Close };

        internal System.Object _content;

        internal Types _type;

        internal System.Object _owner;

        internal Dictionary<string, Chain> _chains;

        internal Dictionary<string, object> _attributes;

        // An iteration that has nothing to iterate.               // 1.2
        //UPGRADE_NOTE: The initialization of  'nullIter' was moved to method 'InitBlock'. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1005"'
        internal static IEnumerator _nullIter;


        //internal long packetNumber = 0; // for debugging only

        private bool isDisposed = false;


        internal Packet(Types newType, string newName, Component newOwner)
        {
            InitBlock();
            _content = newName;
            Owner = newOwner;
            _type = newType;
        }
        internal Packet(System.Object newContent, Component newOwner)
        {
            InitBlock();
            _content = newContent;
            Owner = newOwner;
            _type = Types.Normal;
        }
        internal Packet(Component newOwner)
        {
            InitBlock();
            //_content = newContent;
            Owner = newOwner;
            _type = Types.Normal;
        }

        /// <summary>Maintains a Chain of Packets attached to this Packet.
        /// A Packet may have multiple named Chains attached to it, accessed via a Hashtable.
        /// Since Packets are attached to Chains, as well as Chains to Packets,
        /// this results in an alternation of Packets and Chains, creating a tree structure.
        /// </summary>

        internal virtual void Attach(string name, Packet subordinate)
        {
            if (subordinate == null)
            {
                FlowError.Complain("Null packet reference in 'Attach' method call");
            }
            Packet p = this;
    while (p._owner is Packet) {
      if (p == subordinate) {
        FlowError.Complain("Loop in tree structure");
      }
      p = (Packet) p._owner;
    }
    if (p == subordinate) {
      FlowError.Complain("Loop in tree structure");
    }
    if (p._owner != Thread.CurrentThread)
    {
      FlowError.Complain("Packet not owned (directly or indirectly) by current component");
    }
    if (subordinate._owner != Thread.CurrentThread)
    {
      FlowError.Complain("Subordinate packet not owned by current component");
    }
            if (_chains == null)
                _chains = new Dictionary<string, Chain>();
            Chain chain = (Chain)(_chains[name]);
            if (chain == null)
            {
                chain = new Chain(name);
                _chains.Add(name, chain);
            }
            
            subordinate.Owner = this;
            chain.Members.Add(subordinate);
        }

        /// <summary>Clear the owner of a Packet, and reduce the number of Packets owned by the owner
        /// (if owner is a Component) - if Packet is chained, owner is Chain
        /// </summary>

        internal virtual void ClearOwner()
        {
            if (_owner is Component)
            {
                Component c = (Component)(_owner);
                c._packetCount--;

                //  if (Network.debugging)
                // {
                //      c.ownedPackets.Remove(this.packetNumber);
                //  }
            }
            _owner = null;
        }
        /// <summary>Detach Packet from named chain
        /// </summary>

        internal virtual bool Detach(string name, Packet subordinate)
        {
            if (_chains == null)
                return false;
            Chain chain = (Chain)(_chains[name]);
            if (chain == null)
                return false;
            if (!(chain.Members.Contains(subordinate)))
                return false;
            chain.Members.Remove(subordinate);
            //Packet root = Root;
            //subordinate.Owner = root._owner;
            return true;
        }
         


        /// <summary>Get all attributes of this Packet (as IEnumerator)
        /// </summary>
        /// <summary>Get named chain (as IEnumerator)
        /// </summary>
        /*
                internal virtual IEnumerator getChain(string name)
                {
                    if (_chains == null)
                        return _nullIter;
                    Chain chain = (Chain)(_chains[name]);
                    if (chain != null)
                        return chain.Members.GetEnumerator();
                    else
                        return _nullIter;
                }
                /// <summary>Get all chains for this Packet (as IEnumerator)
                /// </summary>
                internal IEnumerator getChains()
                {
                    if (_chains != null)
                        return (_chains.Keys.GetEnumerator());
                    else
                        return _nullIter;
                }
         * */
        /*
        /// <summary>Get contents of this Packet - may be any Object; if <code>type</code> is not
        /// <code>PacketTypes.Normal</code> it returns <code>null</code>
        /// </summary>
        internal Object GetContent()
        {
            if (_type == PacketTypes.Normal)
                return _content;
            else
                return null;
        }
         * */
        /*
        /// <summary>Get contents of this Packet for case where <code>type</code> is not
        /// <code>PacketTypes.Normal</code>; if it is <code>PacketTypes.Normal</code>, it returns <code>null</code>
        /// </summary>
        internal String GetName()
        {
            if (_type == PacketTypes.Normal)
                return null;
            else
                return (String)(_content);
        }
         *  
        /// <summary>Get root of this Packet - it follows the Packet owner chain up until
        /// it finds a Packet that is owned by a Component rather than by a Chain
        /// </summary>
        Packet GetRoot()
        {
            Packet p = this;
            while (p._owner is Chain)
                p = ((Chain)(p._owner)).Head;
            return p;
        }
         
        /// <summary>This method returns the type of a Packet
        /// </summary>
        internal int GetType()
        {
            return _type;
        }
         * */

        /// <summary>Make an Object a named attribute of a Packet
        /// </summary>

        internal virtual void PutAttribute(string key, System.Object value_Renamed)
        {
            if (_attributes == null)
                _attributes = new Dictionary<string, object>();
            _attributes.Add(key, value_Renamed);
        }
        /// <summary>Remove a named attribute from a Packet (does not return the attribute)
        /// </summary>

        internal virtual void RemoveAttribute(string key)
        {
            if (_attributes != null)
                _attributes.Remove(key);
        }
        // tostring
        public override string ToString()
        {
            string value;
            if (Type == Types.Normal)
                value = (Content == null) ? "null" : Content.ToString();
            else
            {
                value = Type.ToString();
                value += "; " + Name;
            }

            return String.Format("{{Packet '{0}'}}", value);
            //   Type == PacketTypes.Normal ? Content.ToString() : Type.ToString() + ":" + Name);
        }


        /*
       /// <summary>Change the owner of a Packet - if the owner is a Component,
       /// increment the number of Packets owned by that Component
       /// (when the Component is deactivated, it must no longer own any Packets)
       /// </summary>
       void SetOwner(Object newOwner)
       {
           ClearOwner();
           owner = newOwner;
           if (owner is Component)
           {
               Component c = (Component)(owner);
               c._packetCount++;  // count of owned packets

               //		if (Network.debugging) {
               //			if (c.ownedPackets == null)
               //			    c.ownedPackets = new HashMap<String, Packet>();          
               //			c.ownedPackets.put(new Long(c.packetNumber), this);
               //			this.packetNumber = c.packetNumber;
               //			c.packetNumber++;    // unique packet ident for Component
               //		    }
           }

       }
       */
        
   
     ~Packet()
     {
       Dispose(false);
     }
   
     protected void Dispose(bool disposing)
     {
       if (disposing)
       {
         // Code to dispose the managed resources of the class
       }
       // Code to dispose the un-managed resources of the class
   
       isDisposed = true;
     }
   
     public void Dispose()
     {
       Dispose(true);
       GC.SuppressFinalize(this);
     }

    }
}
