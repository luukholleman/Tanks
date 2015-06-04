using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Pathfinding
{
    public class GraphNode
    {
        public int Index;
        
        public readonly Vector2 Position;

        public List<GraphEdge> Edges = new List<GraphEdge>(); 

        public GraphNode(int index, Vector2 position)
        {
            Index = index;
            Position = position;
        }

        public GraphNode(Vector2 position)
        {
            Position = position;
        }

        public bool HasEdgeTo(GraphNode node)
        {
            return Edges.Any(e => e.To == node.Index);
        }
    }
}