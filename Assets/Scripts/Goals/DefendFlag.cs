using System;
using Assets.Scripts.StateMachines.Messaging;
using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class DefendFlag : Goal
    {
        public readonly GameObject Flag;

        public DefendFlag(GameObject flag)
        {
            Flag = flag;
        }

        public override void Activate()
        {
            String msg = "Going to defend " + Flag.name;
            Messenger.BroadcastMessage(new Message(Instance, Message.MessageType.ChatMessage, msg));
            Messenger.Dispatch();

            Instance.GetComponentInChildren<ChatBubble>().Text = "Going to defend " + Flag.name;

            AddSubGoal(new WaitAtFlagTillCaptured(Flag));
            AddSubGoal(new FollowPath(Flag.transform.position));
        }

        public override STATUS Process()
        {
            return ProcessSubGoals();
        }

        public override void Terminate()
        {
            
        }
        
        public override bool IsSameGoal(Goal goal)
        {
            if (goal is DefendFlag)
            {
                return Flag == ((DefendFlag)goal).Flag;
            }

            return base.IsSameGoal(goal);
        }
    }
}
