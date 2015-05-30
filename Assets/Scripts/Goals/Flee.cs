using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Pathfinding;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class Flee : Goal
    {
        private GameObject _tank;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public Flee(GameObject tank)
        {
            _tank = tank;
        }

        public override void Activate()
        {
            //String msg = "FLEEING!!! IMMA GET KILLED";
            //Messenger.BroadcastMessage(new Message(Instance, Message.MessageType.ChatMessage, msg));
            //Messenger.Dispatch();

            _steeringBehaviour = ScriptableObject.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

        }

        public override STATUS Process()
        {
            if (_tank == null)
                return SetStatus(STATUS.COMPLETED);

            _rigidbody.AddForce(_steeringBehaviour.Flee(_tank.transform.position));

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
