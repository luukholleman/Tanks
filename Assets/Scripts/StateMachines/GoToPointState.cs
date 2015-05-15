using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Pathfinding;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    class GoToPointState : IState
    {
        private Vector2 _target;

        private GraphNode nextNode;

        private SteeringBehaviour _steeringBehaviour;

        private List<GraphNode> _path;
        
        public GoToPointState(Vector2 target)
        {
            _target = target;
        }

        public void Update(GameObject instance)
        {
            Vector2 steeringForce = Vector2.zero;

            if (Vector2.Distance(instance.transform.position, nextNode.Position) < 1)
            {
                if (_path.Any())
                {
                    nextNode = _path.First();

                    _path.Remove(nextNode);
                }
                else
                {
                    instance.GetComponent<StateMachine>().CurrentState =
                        instance.GetComponent<StateMachine>().PreviousState;
                }
            }

            steeringForce += _steeringBehaviour.Seek(nextNode.Position);

            instance.GetComponent<Rigidbody2D>().AddForce(steeringForce * instance.GetComponent<Vehicle>().MaxSpeed);

        }

        public void Enter(GameObject instance)
        {
            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.Setup(instance);

            AStar aStar = new AStar(Graph.Instance, Graph.Instance.GetNode(instance.transform.position).Index, Graph.Instance.GetNode(_target).Index);

            _path = aStar.Path;

            if (!_path.Any())
            {
                instance.GetComponent<StateMachine>().CurrentState = new PatrolState();
                return;
            }

            nextNode = _path.First();

            _path.Remove(nextNode);
        }

        public void Exit(GameObject instance)
        {
            
        }

        public void DebugDraw(GameObject instance)
        {
            
        }

        public void CollisionEnter(GameObject instance, Collision2D collision)
        {
            
        }

        public void CollisionExit(GameObject instance, Collision2D collision)
        {
            
        }

        public void CollisionStay(GameObject instance, Collision2D collision)
        {
            
        }

        public void TriggerEnter(GameObject instance, Collider2D collider)
        {
            
        }

        public void TriggerExit(GameObject instance, Collider2D collider)
        {
            
        }

        public void TriggerStay(GameObject instance, Collider2D collider)
        {
            
        }

        public void HandleMessage(GameObject instance, Message msg)
        {
            
        }
    }
}
