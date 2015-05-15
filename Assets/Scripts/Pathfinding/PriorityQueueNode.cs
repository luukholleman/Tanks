using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Assets.Scripts.Pathfinding
{
    public class PriorityQueueNode<T>
    {
        public T Value;
        public float Priority;

        public PriorityQueueNode(float priority, T value)
        {
            Priority = priority;
            Value = value;
        }

        public override string ToString()
        {
            return Priority.ToString();
        }
    }
}
