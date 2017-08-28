using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    
    class ModelResult:IList<Dictionary<string,string>>
    {
        private List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        public Dictionary<string, string> this[int index] { get => ((IList<Dictionary<string, string>>)result)[index]; set => ((IList<Dictionary<string, string>>)result)[index] = value; }

        public int Count => ((IList<Dictionary<string, string>>)result).Count;

        public bool IsReadOnly => ((IList<Dictionary<string, string>>)result).IsReadOnly;

        public void Add(Dictionary<string, string> item)
        {
            ((IList<Dictionary<string, string>>)result).Add(item);
        }

        public void Clear()
        {
            ((IList<Dictionary<string, string>>)result).Clear();
        }

        public bool Contains(Dictionary<string, string> item)
        {
            return ((IList<Dictionary<string, string>>)result).Contains(item);
        }

        public void CopyTo(Dictionary<string, string>[] array, int arrayIndex)
        {
            ((IList<Dictionary<string, string>>)result).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Dictionary<string, string>> GetEnumerator()
        {
            return ((IList<Dictionary<string, string>>)result).GetEnumerator();
        }

        public int IndexOf(Dictionary<string, string> item)
        {
            return ((IList<Dictionary<string, string>>)result).IndexOf(item);
        }

        public void Insert(int index, Dictionary<string, string> item)
        {
            ((IList<Dictionary<string, string>>)result).Insert(index, item);
        }

        public bool Remove(Dictionary<string, string> item)
        {
            return ((IList<Dictionary<string, string>>)result).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Dictionary<string, string>>)result).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Dictionary<string, string>>)result).GetEnumerator();
        }
    }
}
