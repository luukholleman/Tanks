using UnityEngine;

namespace Assets.Scripts.StateMachines.Messaging
{
    public class Message
    {
        public enum MessageType
        {
            TankDied,
            ChatMessage,
            DefendingBase
        }

        public GameObject Sender;
        public GameObject Reciever;
        public MessageType Msg;
        public object ExtraInfo;

        public Message(GameObject sender, GameObject reciever, MessageType msg, object extraInfo = null)
        {
            Sender = sender;
            Reciever = reciever;
            Msg = msg;
            ExtraInfo = extraInfo;
        }
        public Message(GameObject sender, MessageType msg, object extraInfo = null)
        {
            Sender = sender;
            Msg = msg;
            ExtraInfo = extraInfo;
        }
    }
}
