using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Goals.Tank
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
            Debug.Log("Waiting for flag " + _flag + " to capture");

            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.Setup(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();
        }

        public override STATUS Process()
        {
            _rigidbody.AddForce(_steeringBehaviour.Stop(_rigidbody.velocity, 3f));

            if (_flag.GetComponent<Flag>().Side == Instance.GetComponent<Vehicle>().Side)
            {
                return SetStatus(STATUS.COMPLETED);
            }

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
