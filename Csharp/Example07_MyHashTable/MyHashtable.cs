using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example07_MyHashtable
{
    internal class MyHashtable
    {
        private const int DEFAULT_SIZE = 100;
        LinkedList<object>[] _bucket = new LinkedList<object>[DEFAULT_SIZE];
        private int tmpHash;

        public void Add(object key, object value)
        {
            tmpHash = Hash(key.ToString());
            if(_bucket[tmpHash] == null)
                _bucket[tmpHash] = new LinkedList<object>();
            _bucket[tmpHash].AddLast(value);
        }
        public bool Contains(object key)
        {
            tmpHash = Hash(key.ToString());
            if(_bucket[tmpHash] != null && _bucket[tmpHash].Count > 0)
                    return true;
            return false;

        }

        public bool TryGetValue(object key, out object value)
        {
            bool isOK = true;
            value = null;
            tmpHash = Hash(key.ToString());

            try
            {
                value = _bucket[tmpHash];
            }
            catch
            {
                isOK = false;
            }
            return isOK;
        }

        public bool Remove(object key)
        {
            tmpHash = Hash(key.ToString());
            if (_bucket[tmpHash] != null)
            {
                _bucket[tmpHash].Clear();
                _bucket[tmpHash]=null;
                return true;
            }
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < _bucket.Length; i++)
            {
                if (_bucket[i] != null)
                {
                    _bucket[i].Clear();
                    _bucket[i] = null;
                }
            }
        }

        private int Hash(string objName)
        {
            tmpHash = 0;
            for (int i = 0; i < objName.Length; i++)
            {
                tmpHash += objName[i];
            }
            tmpHash %= DEFAULT_SIZE;
            return tmpHash;
        }
    }
}
