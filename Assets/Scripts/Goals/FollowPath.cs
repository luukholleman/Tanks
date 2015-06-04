using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class FollowPath : Goal
    {
        private Vector2 _target;

        private AStar _aStar;

        private bool draw;

        private List<GameObject> lines = new List<GameObject>(); 

        private bool _pathSetup;

        public FollowPath(Vector2 position)
        {
            _target = position;
        }

        public override void Activate()
        {
            _aStar = new AStar(Graph.Instance, Graph.Instance.GetNode(Instance.transform.position).Index, Graph.Instance.GetNode(_target).Index);
        }

        public override STATUS Process()
        {
            DrawPath();

            if (!_aStar.Path.Any())
            {
                _aStar.Search();

                Status = STATUS.Active;
                return STATUS.Active;
            }

            if (!_pathSetup && _aStar.Path.Any())
            {
                foreach (GraphNode node in Enumerable.Reverse(_aStar.Path))
                {
                    AddSubGoal(new GoToNode(node));
                }

                _pathSetup = true;
            }

            if (_pathSetup && !SubGoals.Any())
            {
                return SetStatus(STATUS.Completed);
            }

            return ProcessSubGoals();
        }

        void DrawPath()
        {
            if (Input.GetKey(KeyCode.P))
            {
                draw = !draw;

                if (draw)
                {
                    GraphNode last = null;

                    Color color = new Color(Random.value, Random.value, Random.value);

                    foreach (GraphNode node in _aStar.Path)
                    {
                        if (last != null)
                        {
                            lines.Add(Graph.Instance.DrawEdge(node, last, color));
                        }

                        last = node;
                    }

                    GraphNode lastClosed = null;

                    foreach (int node in _aStar.Closed)
                    {
                        if (last != null)
                        {
                            lines.Add(Graph.Instance.DrawNode(Graph.Instance.GetNode(node), color));
                        }
                    }   
                }
            }
        }

        public override void Terminate()
        {
            foreach (GameObject gameObject in lines)
            {
                GameObject.Destroy(gameObject);
            }
        }

    }
}
