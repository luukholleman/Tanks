using System;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class HoldFlag : Goal
    {
        public readonly GameObject Flag;

        public HoldFlag(GameObject flag)
        {
            Flag = flag;
        }

        public override void Activate()
        {
            String msg = "Omw to hold " + Flag.name;
            Messenger.BroadcastMessage(new Message(Instance, Message.MessageType.ChatMessage, msg));
            Messenger.Dispatch();

            Instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            AddSubGoal(new WaitAtFlag(Flag));
            AddSubGoal(new FollowPath(Flag.transform.position));
        }

        public override STATUS Process()
        {
            return ProcessSubGoals();
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
            if (goal is HoldFlag)
            {
                return Flag == ((HoldFlag)goal).Flag;
            }

            return base.IsSameGoal(goal);
        }
    }
}
