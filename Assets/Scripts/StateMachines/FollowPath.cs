using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Pathfinding;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    class FollowPath : IState
    {
        private List<GraphNode> _path;

        private GraphNode _target;

        private Graph _graph;

        private SteeringBehaviour _steeringBehaviour;

        public FollowPath(List<GraphNode> path)
        {
            _path = path;
            _graph = Graph.Instance;
        }

        public void Update(GameObject instance)
        {
            Vector2 steeringForce = Vector2.zero;
            
            if (Vector2.Distance(instance.transform.position, _target.Position) < 1)
            {
                NextTarget();
            }

            steeringForce += _steeringBehaviour.Seek(_target.Position);

            instance.GetComponent<Rigidbody2D>().AddForce(steeringForce * instance.GetComponent<Vehicle>().MaxSpeed);

        }

        public void Enter(GameObject instance)
        {
            NextTarget();

            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(instance);
        }

        private void NextTarget()
        {
            if (_path.Any())
            {
                _target = _path.First();

                _path.Remove(_target);
            }
            else
            {
                AStar aStar = new AStar(Graph.Instance, Graph.Instance.GetNode(_target.Position).Index, (int)(Random.value * 2000));

                _path = aStar.Path;

                NextTarget();
            }
        }

        public void Exit(GameObject instance)
        {
            
        }

        public void DebugDraw(GameObject instance)
        {
            GraphNode last = null;

            foreach (GraphNode node in _path)
            {
                if(last != null)
                    Debug.DrawLine(last.Position, node.Position, Color.red, Time.fixedDeltaTime);

                last = node;
            }
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
