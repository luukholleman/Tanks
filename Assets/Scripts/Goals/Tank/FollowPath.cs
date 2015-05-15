using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Goals.Tank
{
    class FollowPath : Goal
    {
        private Vector2 _target;

        private Pathfinding.AStar _aStar;

        private bool _pathSetup;

        public FollowPath(Vector2 position)
        {
            _target = position;
        }

        public override void Activate()
        {
            Debug.Log("Following path from " + Instance.transform.position.x + ", " + Instance.transform.position.y + " to " + _target.x + ", " + _target.y);

            _aStar = new AStar(Graph.Instance, Graph.Instance.GetNode(Instance.transform.position).Index, Graph.Instance.GetNode(_target).Index);
        }

        public override STATUS Process()
        {
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

        public override void Terminate()
        {
            Debug.Log("Followed path to " + _target.x + ", " + _target.y);
        }

        public override bool HandleMessage()
        {
            return true;
        }
        
    }
}
