﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class WaitAtFlag : Goal
    {
        public readonly GameObject Flag;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public WaitAtFlag(GameObject flag)
        {
            Flag = flag;
        }

        public override void Activate()
        {
            String msg = "Holding " + Flag.name;
            Messenger.BroadcastMessage(new Message(Instance, Message.MessageType.ChatMessage, msg));
            Messenger.Dispatch();

            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();
        }

        public override STATUS Process()
        {
            _rigidbody.velocity = new Vector2(0, 0);

            if (Vector2.Distance(Instance.transform.position, Flag.transform.position) > Flag.GetComponent<Flag>().CappingRange)
            {
                // somehow we got out of the range of the flag (probably shot out), we need to get back in range
                if ((SubGoals.Any() && SubGoals.Peek().GetType() != typeof(CaptureFlag)) || !SubGoals.Any())
                    AddSubGoal(new CaptureFlag(Flag));

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

        public override bool IsSameGoal(Goal goal)
        {
            if (goal is WaitAtFlag)
            {
                return Flag == ((WaitAtFlag)goal).Flag;
            }

            return base.IsSameGoal(goal);
        }
    }
}
