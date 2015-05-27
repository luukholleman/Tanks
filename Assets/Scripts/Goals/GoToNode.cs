using System.Collections.Generic;
using Assets.Scripts.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class GoToNode : Goal
    {
        private GraphNode _node;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public GoToNode(GraphNode node)
        {
            _node = node;
        }

        public override void Activate()
        {
            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

        }

        public override STATUS Process()
        {
            //Debug.Log("Process Go to Node " + _node.Position.x + ", " + _node.Position.y);

            if (Vector2.Distance(Instance.transform.position, _node.Position) < 1f)
            {
                return SetStatus(STATUS.COMPLETED);
            }

            Vector2 steeringForce = Vector2.zero;
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Instance.transform.position, 2f, LayerMask.GetMask("Tank"));

            List<GameObject> neighbours = new List<GameObject>();

            foreach (Collider2D collider2D in colliders)
            {
                if (collider2D.gameObject != Instance.gameObject)
                    neighbours.Add(collider2D.gameObject);
            }

            steeringForce += _steeringBehaviour.Alignment(neighbours) * Instance.transform.GetComponent<Vehicle>().MaxSpeed;
            steeringForce += _steeringBehaviour.Cohesion(neighbours) * Instance.transform.GetComponent<Vehicle>().MaxSpeed;
            steeringForce += _steeringBehaviour.Separation(neighbours) * Instance.transform.GetComponent<Vehicle>().MaxSpeed;

            steeringForce = steeringForce.normalized * Instance.transform.GetComponent<Vehicle>().MaxSpeed;

            Instance.GetComponent<Rigidbody2D>().AddForce(steeringForce);

            _rigidbody.AddForce(_steeringBehaviour.Seek(_node.Position) * Instance.GetComponent<Vehicle>().MaxSpeed);

            return SetStatus(STATUS.ACTIVE);
        }

        public override void Terminate()
        {
            
        }

        public override bool HandleMessage()
        {
            return true;
        }
    }
}
