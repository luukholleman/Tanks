using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Pathfinding
{
    class AStar
    {
        private readonly Graph _graph;

        public List<GraphNode> Path = new List<GraphNode>();

        private readonly int _target;

        private readonly int[] _previous;
        private readonly float[] _distances;

        public List<int> Closed = new List<int>();
        //PriorityQueue<int> Open = new PriorityQueue<int>();

        readonly PriorityQueue Open = new PriorityQueue();

        int _iterationCount;

        private int _iterationsPerCall = 20;

        Stopwatch sw = new Stopwatch();
        public AStar(Graph graph, int source, int target)
        {
            _graph = graph;
            _target = target;

            _previous = new int[_graph.NodeCount];
            _distances = new float[_graph.NodeCount];

            foreach (var vertex in _graph.GetNodes())
            {
                if (vertex.Index == source)
                    _distances[vertex.Index] = 0;
                else
                    _distances[vertex.Index] = float.MaxValue;
            }

            // queue starting node
            Open.Enqueue(source, 0);
        }

        public bool Search()
        {
            int currentIterations = 0;

            sw.Start();

            while (!Open.IsEmpty())
            {
                _iterationCount++;

                var smallest = (int)Open.Dequeue();

                // we've found the path
                if (smallest == _target)
                {
                    // create path as list
                    while (_previous[smallest] != 0)
                    {
                        Path.Add(_graph.GetNode(smallest));
                        smallest = _previous[smallest];
                    }

                    sw.Stop();

                    //Debug.Log("Path found!!");
                    //Debug.Log("Iterations: " + _iterationCount);
                    //Debug.Log(sw.Elapsed);

                    // path is reversed
                    Path.Reverse();

                    return true;
                }

                // can't find a path
                if (_distances[smallest] == float.MaxValue)
                {
                    return false;
                }

                foreach (var neighbor in _graph.GetNode(smallest).Edges)
                {
                    var neighbourCost = _distances[smallest] + neighbor.Cost;

                    // is this current path faster than a previous found path?
                    if (neighbourCost < _distances[neighbor.To])
                    {
                        // yes it is! add the new path as solution
                        _distances[neighbor.To] = neighbourCost;
                        _previous[neighbor.To] = smallest;
                    }

                    // did we already check this node? if so, it can't be faster to check again
                    if (!Closed.Exists(n => n == neighbor.To))
                    {
                        // calc distance to target as a* heuristic
                        float distance = Vector2.Distance(_graph.GetNode(_target).Position, _graph.GetNode(smallest).Position);
                        
                        // enqueue neighbour
                        Open.Enqueue(neighbor.To, (int)(neighbourCost + distance + Random.value / 2));
                    }

                    // we've checked this node, never check it again
                    Closed.Add(neighbor.To);
                }

                // limit iterations per update to increase performance
                if (++currentIterations >= _iterationsPerCall)
                    return false;  
            }

            return false;
        }
    }
}
