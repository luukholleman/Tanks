using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Goals.Tank
{
    class Wander : Goal
    {
        private SteeringBehaviour _steeringBehaviour;

        public override void Activate()
        {
            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(Instance);
        }

        public override STATUS Process()
        {
            Instance.GetComponent<Rigidbody2D>().AddForce(_steeringBehaviour.Wander());

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
