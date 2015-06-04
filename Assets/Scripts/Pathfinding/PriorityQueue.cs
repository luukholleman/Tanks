using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Pathfinding
{
    public class PriorityQueue
    {
        private int _size;
        SortedDictionary<int, Queue> dict;

        public PriorityQueue()
        {
            dict = new SortedDictionary<int, Queue>();
            _size = 0;
        }

        public bool IsEmpty()
        {
            return (_size == 0);
        }

        public object Dequeue()
        {
            if (IsEmpty())
                return null;

            _size--;
            return dict.Values.First(q => q.Count > 0).Dequeue();
        }

        public object Peek()
        {
            if (IsEmpty())
                return null;

            return dict.Values.First(q => q.Count > 0).Peek();
        }

        public object Dequeue(int key)
        {
            _size--;
            return dict[key].Dequeue();
        }

        public void Enqueue(object item, int key)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, new Queue());

            _size++;
            dict[key].Enqueue(item);
        }
    }
}
