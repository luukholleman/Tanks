using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Pathfinding
{
    class AStar
    {
        private readonly Graph _graph;

        public List<GraphNode> Path = new List<GraphNode>();

        private int _source;

        private int _target;

        Dictionary<int, int> _previous = new Dictionary<int, int>();
        Dictionary<int, float> _distances = new Dictionary<int, float>();
        List<int> closed = new List<int>();
        PriorityQueue<int> pq = new PriorityQueue<int>();

        int i = 0;

        private int _iterationsPerCall = 25;

        Stopwatch sw = new Stopwatch();
        public AStar(Graph graph, int source, int target)
        {
            _graph = graph;
            _source = source;
            _target = target;

            foreach (var vertex in _graph.GetNodes())
            {
                if (vertex.Index == _source)
                {
                    _distances[vertex.Index] = 0;
                }
                else
                {
                    _distances[vertex.Index] = 900001;
                }
            }

            pq.Enqueue(new PriorityQueueNode<int>(0, _source));
        }

        public bool Search()
        {
            int j = 0;

            sw.Start();
            while (!pq.Empty())
            {
                i++;

                var smallest = pq.DeQueue().Value;

                if (smallest == _target)
                {
                    while (_previous.ContainsKey(smallest))
                    {
                        Path.Add(_graph.GetNode(smallest));
                        smallest = _previous[smallest];
                    }

                    sw.Stop();
                    Debug.Log("Path found!!");
                    Debug.Log("Iterations: " + i);
                    Debug.Log(sw.Elapsed);

                    Path.Reverse();

                    return true;
                }

                if (_distances[smallest] == 900001)
                {
                    return false;
                }

                foreach (var neighbor in _graph.GetNode(smallest).Edges)
                {
                    float distance = Vector2.Distance(_graph.GetNode(_target).Position,
                        _graph.GetNode(smallest).Position);

                    var alt = _distances[smallest] + neighbor.Cost + Random.value / 10;

                    if (alt < _distances[neighbor.To])
                    {
                        _distances[neighbor.To] = alt;
                        _previous[neighbor.To] = smallest;
                    }

                    if (!closed.Exists(n => n == neighbor.To))
                        pq.Enqueue(new PriorityQueueNode<int>(alt + distance, neighbor.To));

                    closed.Add(neighbor.To);
                }

                if (++j >= _iterationsPerCall)
                {
                    return false;   
                }
            }

            return false;
        }
        //private bool Search()
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    var previous = new Dictionary<int, int>();
        //    var distances = new Dictionary<int, float>();
        //    var nodes = new List<int>();
        //    var closed = new List<int>();

        //    PriorityQueue<int> pq = new PriorityQueue<int>();

        //    foreach (var vertex in _graph.GetNodes())
        //    {
        //        if (vertex.Index == _source)
        //        {
        //            distances[vertex.Index] = 0;
        //        }
        //        else
        //        {
        //            distances[vertex.Index] = 900001;
        //        }

        //        //nodes.Add(vertex.Index);
        //    }

        //    //nodes.Add(_graph.GetNode(_source).Index);

        //    pq.Enqueue(new PriorityQueueNode<int>(0, _source));

        //    int i = 0;
        //    while (!pq.Empty())
        //    {
        //        i++;
        //        //nodes.Sort((x, y) => (int)(distances[x] - distances[y]));

        //        var smallest = pq.DeQueue().Value;

        //        //var smallest = nodes[0];
        //        //nodes.Remove(smallest);

        //        if (smallest == _target)
        //        {
        //            while (previous.ContainsKey(smallest))
        //            {
        //                Path.Add(_graph.GetNode(smallest));
        //                smallest = previous[smallest];
        //            }

        //            sw.Stop();
        //            Debug.Log("Path found!!");
        //            Debug.Log("Iterations: " + i);
        //            Debug.Log(sw.Elapsed);

        //            Path.Reverse();

        //            return true;
        //        }

        //        if (distances[smallest] == 900001)
        //        {
        //            return false;
        //        }

        //        foreach (var neighbor in _graph.GetNode(smallest).Edges)
        //        {
        //            float distance = Vector2.Distance(_graph.GetNode(_target).Position,
        //                _graph.GetNode(smallest).Position);

        //            var alt = distances[smallest] + neighbor.Cost;

        //            if (alt < distances[neighbor.To])
        //            {
        //                distances[neighbor.To] = alt;
        //                previous[neighbor.To] = smallest;
        //            }

        //            if (!closed.Exists(n => n == neighbor.To))
        //                pq.Enqueue(new PriorityQueueNode<int>(alt + distance, neighbor.To));

        //            closed.Add(neighbor.To);
        //        }
        //    }

        //    return false;
        //}
    }
}
