using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.StateMachines.Messaging
{
    class Messenger
    {
        private static List<Message> _messages = new List<Message>(); 
        public static bool SendMessage(Message msg)
        {
            if (msg.Reciever.GetComponent<StateMachine>() != null)
            {
                _messages.Add(msg);

                return true;
            }

            return false;
        }

        public static void BroadcastMessage(Message msg)
        {
            GameObject[] gameObjects =  GameObject.FindObjectsOfType(typeof (GameObject)) as GameObject[];

            if (gameObjects != null)
                foreach (GameObject gameObject in gameObjects)
                {
                    Message newMsg = new Message(msg.Sender, gameObject, msg.Msg, msg.ExtraInfo);
                    SendMessage(newMsg);
                }
        }

        public static void Dispatch()
        {
            foreach (Message message in _messages)
            {
                message.Reciever.GetComponent<StateMachine>().HandleMessage(message);
            }

            _messages.Clear();
        }
    }
}
