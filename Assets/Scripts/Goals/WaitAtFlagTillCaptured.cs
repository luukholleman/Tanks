using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class WaitAtFlagTillCaptured : Goal
    {
        private GameObject _flag;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public WaitAtFlagTillCaptured(GameObject flag)
        {
            _flag = flag;
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
            Debug.Log("Flag " + _flag + " is captured");
        }

        public override bool HandleMessage()
        {
            return true;
        }
    }
}
