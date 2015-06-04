using UnityEngine;

namespace Assets.Scripts.Goals
{
    class Wander : Goal
    {
        private SteeringBehaviour _steeringBehaviour;

        public override void Activate()
        {
            _steeringBehaviour = new SteeringBehaviour(Instance);
        }

        public override STATUS Process()
        {
            Instance.GetComponent<Rigidbody2D>().AddForce(_steeringBehaviour.Wander());

            return SetStatus(STATUS.Active);
        }

        public override void Terminate()
        {
            
        }
    }
}
