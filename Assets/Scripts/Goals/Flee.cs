using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Pathfinding;
using Assets.Scripts.StateMachines.Messaging;
using Assets.Scripts.Tank;
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

            Instance.GetComponentInChildren<ChatBubble>().Text = "IMMA OUTTA HERE";

            _steeringBehaviour = ScriptableObject.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

        }

        public override STATUS Process()
        {
            if (_tank == null)
                return SetStatus(STATUS.COMPLETED);

            Vector2 steeringForce = Vector2.zero;

            steeringForce += _steeringBehaviour.Flee(_tank.transform.position);
            steeringForce += _steeringBehaviour.ObstacleAvoidance(Physics2D.OverlapCircleAll(Instance.transform.position, 10f, LayerMask.GetMask("Obstacle")));

            _rigidbody.AddForce(steeringForce * 2f);

            return SetStatus(STATUS.ACTIVE);
        }

        public override void Terminate()
        {
            
        }

        public override bool HandleMessage()
        {
            return true;
        }

        public override int CompareTo(object obj)
        {
            return ((Flee)obj)._tank == _tank ? 0 : 1;
        }
    }
}
