using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    class IdlingState : IState
    {
        public void Update(GameObject instance)
        {
            if ((int)(Random.value * 100) == 1)
            {
                instance.GetComponent<StateMachine>().CurrentState = new PatrolState();
            }
        }

        public void Enter(GameObject instance)
        {
            Debug.Log("Entering Idling State");
        }

        public void Exit(GameObject instance)
        {
            Debug.Log("Exiting Idling State");
        }

        public void DebugDraw(GameObject transform)
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
