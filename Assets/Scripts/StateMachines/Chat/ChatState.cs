using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.StateMachines.Messaging;
using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.StateMachines.Chat
{
    class ChatState : IState
    {
        private List<String> _messages = new List<String>();
 
        public override void Update(GameObject instance)
        {

        }

        public override void Enter(GameObject instance)
        {
            _messages.Clear();
        }

        public override void Exit(GameObject instance)
        {
            
        }

        public override void OnGUI()
        {
            int i = 0;
            foreach (String message in Enumerable.Reverse(_messages).Take(10))
            {
                GUI.Label(new Rect(10, i++ * 18 + 10, 500, 18), message);
            }
        }

        public override void HandleMessage(GameObject instance, Message msg)
        {
            if (msg.Msg == Message.MessageType.ChatMessage)
            {
                _messages.Add("[" + msg.Sender.name + "] " + msg.ExtraInfo);
            }
        }
    }
}
