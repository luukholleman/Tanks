using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Pathfinding
{
    public class GraphEdge
    {
        public int To { get; set; }
        public float Cost { get; set; }

        public GraphEdge(int to, float cost)
        {
            To = to;
            Cost = cost;
        }
    }
}
