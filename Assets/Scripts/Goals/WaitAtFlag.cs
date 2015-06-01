using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class WaitAtFlag : Goal
    {
        private GameObject _flag;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public WaitAtFlag(GameObject flag)
        {
            _flag = flag;
        }

        public override void Activate()
        {
            String msg = "Holding " + _flag.name;
            Messenger.BroadcastMessage(new Message(Instance, Message.MessageType.ChatMessage, msg));
            Messenger.Dispatch();

            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();
        }

        public override STATUS Process()
        {
            _rigidbody.velocity = new Vector2(0, 0);

            if (Vector2.Distance(Instance.transform.position, _flag.transform.position) > _flag.GetComponent<Flag>().CappingRange)
            {
                // somehow we got out of the range of the flag (probably shot out), we need to get back in range
                if ((SubGoals.Any() && SubGoals.Peek().GetType() != typeof(CaptureFlag)) || !SubGoals.Any())
                    AddSubGoal(new CaptureFlag(_flag));

                return ProcessSubGoals();
            }

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
            return ((WaitAtFlag)obj)._flag == _flag ? 0 : 1;
        }
    }
}
