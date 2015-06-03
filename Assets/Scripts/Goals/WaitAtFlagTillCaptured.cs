using System;
using System.Linq;
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
            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

            _rigidbody.velocity = Vector2.zero;
        }

        public override STATUS Process()
        {
            _rigidbody.AddForce(_steeringBehaviour.Stop(_rigidbody.velocity, 0.1f));
            
            return SetStatus(STATUS.ACTIVE);
        }

        public override void Terminate()
        {
            Debug.Log("Flag " + Flag + " is captured");
        }

        public override bool HandleMessage()
        {
            return true;
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
