using System;
using Assets.Scripts.StateMachines.Messaging;
using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class DefendFlag : Goal
    {
        private GameObject _flag;

        public DefendFlag(GameObject flag)
        {
            _flag = flag;
        }

        public override void Activate()
        {
            String msg = "Going to defend " + _flag.name;
            Messenger.BroadcastMessage(new Message(Instance, Message.MessageType.ChatMessage, msg));
            Messenger.Dispatch();

            Instance.GetComponentInChildren<ChatBubble>().Text = "Going to defend " + _flag.name;

            AddSubGoal(new WaitAtFlagTillCaptured(_flag));
            AddSubGoal(new FollowPath(_flag.transform.position));
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

        public override int CompareTo(object obj)
        {
            return ((DefendFlag)obj)._flag == _flag ? 0 : 1;
        }
    }
}
