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
            _steeringBehaviour = new SteeringBehaviour(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

        }

        public override STATUS Process()
        {
            if (Vector2.Distance(Instance.transform.position, _node.Position) < 1f)
            {
                return SetStatus(STATUS.Completed);
            }

            Vector2 steeringForce = Vector2.zero;
            
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Instance.transform.position, 2f, LayerMask.GetMask("Tank"));

            List<GameObject> neighbours = new List<GameObject>();

            foreach (Collider2D collider2D in colliders)
            {
                if (collider2D.gameObject != Instance.gameObject)
                    neighbours.Add(collider2D.gameObject);
            }

            steeringForce += _steeringBehaviour.Alignment(neighbours);
            steeringForce += _steeringBehaviour.Cohesion(neighbours);
            steeringForce += _steeringBehaviour.Separation(neighbours);

            steeringForce = steeringForce.normalized * Instance.transform.GetComponent<Tank.Tank>().MaxSpeed;

            steeringForce += _steeringBehaviour.Seek(_node.Position)*Instance.GetComponent<Tank.Tank>().MaxSpeed;

            _rigidbody.AddForce(steeringForce);

            return SetStatus(STATUS.Active);
        }

        public override void Terminate()
        {
            
        }

    }
}
