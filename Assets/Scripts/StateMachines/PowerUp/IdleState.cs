using Assets.Scripts.Pathfinding;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines.PowerUp
{
    public class IdleState : IState
    {

        private float _lastDrop;
        private float _nextDrop;

        public void Update(GameObject instance)
        {
            _lastDrop += Time.fixedDeltaTime;

            if (_lastDrop >= _nextDrop)
            {
                instance.GetComponent<StateMachine>().CurrentState = new DropItemState(Graph.Instance.GetRandomNode().Position);
            }
        }

        public void Enter(GameObject instance)
        {
            _lastDrop = 0;
            _nextDrop = Random.value*5 + 2;
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
