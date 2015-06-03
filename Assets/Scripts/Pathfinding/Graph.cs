using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Pathfinding
{
    public class Graph : MonoBehaviour
    {
        public static Graph Instance;

        public float NodeDistance = 1f;

        public bool Diagonal = true;

        public List<Vector2> _closed = new List<Vector2>(); 

        void Start()
        {
            Instance = this;

            FloodFill(new Vector2(0, 0));
        }

        GraphNode FloodFill(Vector2 position)
        {
            _closed.Add(position);

            Collider2D collider = Physics2D.OverlapArea(new Vector2(position.x - NodeDistance / 2, position.y - NodeDistance / 2), new Vector2(position.x + NodeDistance / 2, position.y + NodeDistance / 2), LayerMask.GetMask("Wall", "Obstacle", "Tree"));

            // er staat hier niks
            if (collider == null)
            {
                GraphNode node = new GraphNode(position);

                AddNode(node);
                
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        Vector2 newPosition = position + new Vector2(x, y);

                        if (!_closed.Any(n => n == newPosition))
                        {
                            FloodFill(newPosition);
                        }
                    }
                }

                List<GraphNode> neighbours = new List<GraphNode>();

                // horizontaal en verticaal
                neighbours.Add(nodes.FirstOrDefault(n => n.Position == node.Position + new Vector2(NodeDistance, 0)));
                neighbours.Add(nodes.FirstOrDefault(n => n.Position == node.Position + new Vector2(0, NodeDistance)));
                neighbours.Add(nodes.FirstOrDefault(n => n.Position == node.Position + new Vector2(0, -NodeDistance)));
                neighbours.Add(nodes.FirstOrDefault(n => n.Position == node.Position + new Vector2(-NodeDistance, 0)));

                if (Diagonal)
                {
                    // diagonaal
                    neighbours.Add(nodes.FirstOrDefault(n => n.Position == node.Position + new Vector2(NodeDistance, NodeDistance)));
                    neighbours.Add(nodes.FirstOrDefault(n => n.Position == node.Position + new Vector2(-NodeDistance, NodeDistance)));
                    neighbours.Add(nodes.FirstOrDefault(n => n.Position == node.Position + new Vector2(NodeDistance, -NodeDistance)));
                    neighbours.Add(nodes.FirstOrDefault(n => n.Position == node.Position + new Vector2(-NodeDistance, -NodeDistance)));
                }

                foreach (GraphNode neighbour in neighbours.Where(n => n != null))
                {
                    node.Edges.Add(new GraphEdge(neighbour.Index, NodeDistance));
                }

                return node;
            }

            return null;
        }

        void Update()
        {
        }

        public GameObject DrawEdge(GraphNode node1, GraphNode node2, Color color)
        {
            GameObject parent = GameObject.Find("Common/Graph");

            GameObject quadLine =
                Instantiate(Resources.Load<GameObject>("PreFabs/Line"), node1.Position, new Quaternion()) as
                    GameObject;

            float angle =
                (float)
                    Math.Atan2(node1.Position.y - node2.Position.y, node1.Position.x - node2.Position.x) *
                (180 / (float)Math.PI);

            angle += 90;

            quadLine.transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector3 newPosition = node1.Position + (node2.Position - node1.Position) / 2;

            quadLine.transform.position = newPosition;

            float dist = Vector2.Distance(node1.Position, node2.Position);

            quadLine.transform.localScale = new Vector3(0.1f, dist, 1f);

            quadLine.transform.parent = parent.transform;

            quadLine.GetComponent<Renderer>().material.color = color;

            return quadLine;
        }

        public static readonly int InvalidNodeIndex = -1;

        private List<GraphNode> nodes = new List<GraphNode>();

        public int NextIndex { get { return _nextIndex++; } }

        private int _nextIndex = 0;

        public int NodeCount
        {
            get { return nodes.Count; }
        }

        public int ActiveNodeCount
        {
            get { return nodes.Count(n => n.Index != InvalidNodeIndex); }
        }

        public bool IsEmpty
        {
            get { return !nodes.Any(); }
        }

        public GraphNode GetNode(int index)
        {
            GraphNode node = nodes[index];

            if(node.Index != InvalidNodeIndex)
                return nodes[index];

            return null;
        }

        public GraphNode GetNode(Vector2 position)
        {
            GraphNode bestNode = null;
            float dist = float.MaxValue;

            foreach (GraphNode node in nodes)
            {
                if (Vector2.Distance(position, node.Position) < dist)
                {
                    bestNode = node;
                    dist = Vector2.Distance(position, node.Position);
                }
            }

            return bestNode;
        }

        public List<GraphNode> GetNodes()
        {
            return nodes;
        }

        public int AddNode(GraphNode node)
        {
            node.Index = NextIndex;
            nodes.Add(node);

            return node.Index;
        }

        public void RemoveNode(int index)
        {
            nodes[index].Index = InvalidNodeIndex;
        }

        public bool IsPresent(int index)
        {
            return nodes.Any(n => n.Index == index);
        }

        public GraphNode GetRandomNode()
        {
            return GetNode((int)(Random.value * NodeCount));
        }
    }
}
