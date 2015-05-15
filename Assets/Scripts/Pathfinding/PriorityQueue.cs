using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Pathfinding
{
    public class PriorityQueue<T>
    {
        public List<PriorityQueueNode<T>> items = new List<PriorityQueueNode<T>>();

        public void Enqueue(PriorityQueueNode<T> item)
        {
            items.Add(item);
        }

        public PriorityQueueNode<T> DeQueue()
        {
            items.Sort((x, y) => (int)(x.Priority - y.Priority));

            PriorityQueueNode<T> item = items[0];

            items.Remove(item);

            return item;
        }

        public bool Empty()
        {
            return !items.Any();
        }
    }
}
