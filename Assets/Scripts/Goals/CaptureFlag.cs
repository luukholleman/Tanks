using System;
using Assets.Scripts.StateMachines.Messaging;
using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class CaptureFlag : Goal
    {
        private GameObject _flag;

        public CaptureFlag(GameObject flag)
        {
            _flag = flag;
        }

        public override void Activate()
        {
            String msg = "Going to attack " + _flag.name;

            Messenger.BroadcastMessage(new Message(Instance, Message.MessageType.ChatMessage, msg));

            Messenger.Dispatch();

            Instance.GetComponentInChildren<ChatBubble>().Text = "Going to capture " + _flag.name;

            AddSubGoal(new WaitAtFlagTillCaptured(_flag));
            AddSubGoal(new FollowPath(_flag.transform.position));
        }

        public override STATUS Process()
        {
            if (_flag.GetComponent<Flag>().Side == Instance.GetComponent<Vehicle>().Side && Math.Abs(_flag.GetComponent<Flag>().Score) == _flag.GetComponent<Flag>().MaxScore)
            {
                return SetStatus(STATUS.COMPLETED);
            }

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
            return ((CaptureFlag)obj)._flag == _flag ? 0 : 1;
        }
    }
}
