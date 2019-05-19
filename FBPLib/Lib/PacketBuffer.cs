/// <summary> This class is founded on Doug Lea's BoundedBuffer class
/// from his book _Concurrent Programming in Java_.
/// </summary>
namespace FBPLib
{
    using System;

    internal class PacketBuffer
    {
        /* *
          * Copyright 2007, 2008, J. Paul Morrison.  At your option, you may copy, 
          * distribute, or make derivative works under the terms of the Clarified Artistic License, 
          * based on the Everything Development Company's Artistic License.  A document describing 
          * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
          * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
          * */
        // The packets currently in transit.
        internal Packet[] _array;

        // Index into array where the next packet sent should go.
        internal int _sendPtr = 0;

        // Index into array where the next packet received should come from.
        internal int _receivePtr = 0;

        // Number of slots in array currently in use.
        internal int _usedSlots = 0;


        internal PacketBuffer(int size)
        {
            _array = new Packet[size];
        }
        /// <summary>Returns the number of packets this buffer can hold.
        /// </summary>
        internal virtual int Capacity()
        {
            return _array.Length;
        }
        /// <summary>Return the number of packets currently in this buffer.
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'count'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        internal virtual int Count()
        {
            lock (this)
            {
                return _usedSlots;
            }
        }

        internal virtual void Put(Packet x)
        {
            _array[_sendPtr] = x;
            if (++_sendPtr >= _array.Length)
                _sendPtr = 0;
            IncUsedSlots();
        }
        internal virtual Packet Take()
        {
            --_usedSlots;
            Packet old = _array[_receivePtr];
            _array[_receivePtr] = null;
            if (++_receivePtr >= _array.Length)
                _receivePtr = 0;
            return old;
        }

        //UPGRADE_NOTE: Synchronized keyword was removed from method 'incUsedSlots'. Lock expression was added. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1027"'
        protected internal virtual void IncUsedSlots()
        {
            lock (this)
            {
                ++_usedSlots;
               //  System.Threading.Monitor.Pulse(this);  
            }
        }
    }
}