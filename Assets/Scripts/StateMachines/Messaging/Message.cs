using UnityEngine;

namespace Assets.Scripts.StateMachines.Messaging
{
    public class Message
    {
        public enum MessageType
        {
            TankDied,
            PowerUpDropped
        }

        public GameObject Sender;
        public GameObject Reciever;
        public MessageType Msg;
        public object ExtraInfo;

        public Message(GameObject sender, GameObject reciever, MessageType msg)
        {
            Sender = sender;
            Reciever = reciever;
            Msg = msg;
        }
        public Message(GameObject sender, MessageType msg)
        {
            Sender = sender;
            Msg = msg;
        }
    }
}
