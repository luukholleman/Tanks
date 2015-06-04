using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class Flee : Goal
    {
        public readonly GameObject Tank;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public Flee(GameObject tank)
        {
            Tank = tank;
        }

        public override void Activate()
        {
            Instance.GetComponentInChildren<ChatBubble>().Text = "IMMA OUTTA HERE";

            _steeringBehaviour = new SteeringBehaviour(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

        }

        public override STATUS Process()
        {
            if (Tank == null)
                return SetStatus(STATUS.Completed);

            Vector2 steeringForce = Vector2.zero;

            steeringForce += _steeringBehaviour.Flee(Tank.transform.position);
            steeringForce += _steeringBehaviour.ObstacleAvoidance(Physics2D.OverlapCircleAll(Instance.transform.position, 10f, LayerMask.GetMask("Obstacle")));

            _rigidbody.AddForce(steeringForce * 2f);

            return SetStatus(STATUS.Active);
        }

        public override void Terminate()
        {
            
        }

        public override bool IsSameGoal(Goal goal)
        {
            if (goal is Flee)
            {
                return Tank == ((Flee)goal).Tank;
            }

            return base.IsSameGoal(goal);
        }
    }
}
