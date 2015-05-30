using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class FollowPath : Goal
    {
        private Vector2 _target;

        private Pathfinding.AStar _aStar;

        private bool draw = false;

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

                Status = STATUS.ACTIVE;
                return STATUS.ACTIVE;
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
                return SetStatus(STATUS.COMPLETED);
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

                    foreach (GraphNode node in _aStar.Path)
                    {
                        if (last != null)
                        {
                            lines.Add(Graph.Instance.DrawEdge(node, last, Color.red));
                        }

                        last = node;
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

        public override bool HandleMessage()
        {
            return true;
        }
        
    }
}
