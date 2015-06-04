using System;
using Assets.Scripts.StateMachines.Messaging;
using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class CaptureFlag : Goal
    {
        public readonly GameObject Flag;

        public CaptureFlag(GameObject flag)
        {
            Flag = flag;
        }

        public override void Activate()
        {
            String msg = "Going to attack " + Flag.name;

            Messenger.BroadcastMessage(new Message(Instance, Message.MessageType.ChatMessage, msg));

            Messenger.Dispatch();

            Instance.GetComponentInChildren<ChatBubble>().Text = "Going to capture " + Flag.name;

            AddSubGoal(new WaitAtFlagTillCaptured(Flag));
            AddSubGoal(new FollowPath(Flag.transform.position));
        }

        public override STATUS Process()
        {
            if (Flag.GetComponent<Flag>().Side == Instance.GetComponent<Tank.Tank>().Side && Math.Abs(Flag.GetComponent<Flag>().Score) == Flag.GetComponent<Flag>().MaxScore)
            {
                return SetStatus(STATUS.Completed);
            }

            return ProcessSubGoals();
        }

        public override void Terminate()
        {

        }
        
        public override bool IsSameGoal(Goal goal)
        {
            if (goal is CaptureFlag)
            {
                return Flag == ((CaptureFlag)goal).Flag;
            }

            return base.IsSameGoal(goal);
        }
    }
}
