using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines.PowerUp
{
    public class DropItemState : IState
    {
        private Vector3 _position;
        public DropItemState(Vector3 position)
        {
            _position = position;
        }

        public void Update(GameObject instance)
        {
            int val = (int)(Random.value * 10);

            GameObject powerUp;

            if (val <= 3)
                powerUp = GameObject.Instantiate(Resources.Load<GameObject>("PreFabs/PowerUps/Movement"), _position, new Quaternion()) as GameObject;
            else if (val <= 6)
                powerUp = GameObject.Instantiate(Resources.Load<GameObject>("PreFabs/PowerUps/Reload"), _position, new Quaternion()) as GameObject;
            else if (val <= 9)
                powerUp = GameObject.Instantiate(Resources.Load<GameObject>("PreFabs/PowerUps/Repair"), _position, new Quaternion()) as GameObject;
            else
                powerUp = GameObject.Instantiate(Resources.Load<GameObject>("PreFabs/PowerUps/ExtraTank"), _position, new Quaternion()) as GameObject;

            powerUp.transform.parent = GameObject.Find("PowerUps").transform;

            // notify surrounding vehicles
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_position, 20, LayerMask.GetMask("Tank", "Robot"));

            foreach (Collider2D collider in colliders)
            {
                Message msg = new Message(powerUp, collider.gameObject, Message.MessageType.PowerUpDropped);

                Messenger.SendMessage(msg);
            }

            Messenger.Dispatch();

            // never run this method twice, return to previous state
            instance.GetComponent<StateMachine>().CurrentState = instance.GetComponent<StateMachine>().PreviousState;
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
        
        }
    }
}
