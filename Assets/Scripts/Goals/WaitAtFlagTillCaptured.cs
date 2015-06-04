using UnityEngine;

namespace Assets.Scripts.Goals
{
    class WaitAtFlagTillCaptured : Goal
    {
        public readonly GameObject Flag;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public WaitAtFlagTillCaptured(GameObject flag)
        {
            Flag = flag;
        }

        public override void Activate()
        {
            _steeringBehaviour = new SteeringBehaviour(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();
        }

        public override STATUS Process()
        {
            _rigidbody.AddForce(_steeringBehaviour.Orbit(Flag.transform.position));
            
            return SetStatus(STATUS.Active);
        }

        public override void Terminate()
        {
            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector2.zero;
            }
        }

        public override bool IsSameGoal(Goal goal)
        {
            if (goal is WaitAtFlagTillCaptured)
            {
                return Flag == ((WaitAtFlagTillCaptured)goal).Flag;
            }

            return base.IsSameGoal(goal);
        }
    }
}
