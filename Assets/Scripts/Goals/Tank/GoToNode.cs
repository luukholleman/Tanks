using Assets.Scripts.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Goals.Tank
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

            _steeringBehaviour.Setup(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

        }

        public override STATUS Process()
        {
            //Debug.Log("Process Go to Node " + _node.Position.x + ", " + _node.Position.y);

            if (Vector2.Distance(Instance.transform.position, _node.Position) < 1f)
            {
                return SetStatus(STATUS.COMPLETED);
            }

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
