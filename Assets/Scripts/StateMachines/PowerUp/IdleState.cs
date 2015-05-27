using Assets.Scripts.Pathfinding;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines.PowerUp
{
    public class IdleState : IState
    {

        public void Update(GameObject instance)
        {

        }

        public void Enter(GameObject instance)
        {

        }

        public void Exit(GameObject instance)
        {
            
        }

        public void DebugDraw(GameObject instance)
        {
            
        }

        public void CollisionEnter(GameObject instance, Collision2D collision)
        {
            
        }

        public void CollisionExit(GameObject instance, Collision2D collision)
        {
            
        }

        public void CollisionStay(GameObject instance, Collision2D collision)
        {
            
        }

        public void TriggerEnter(GameObject instance, Collider2D collider)
        {
            
        }

        public void TriggerExit(GameObject instance, Collider2D collider)
        {
            
        }

        public void TriggerStay(GameObject instance, Collider2D collider)
        {
            
        }

        public void HandleMessage(GameObject instance, Message msg)
        {
            if (msg.Msg == Message.MessageType.TankDied)
                instance.GetComponent<StateMachine>().CurrentState = new DropItemState(msg.Sender.transform.position);
        }
    }
}
