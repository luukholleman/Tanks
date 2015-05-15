using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.StateMachines
{
    class PatrolState : IState
    {
        private SteeringBehaviour _steeringBehaviour;
        
        public void Update(GameObject instance)
        {
            Collider2D collider = Physics2D.OverlapCircle(instance.transform.position, 20f, LayerMask.GetMask("PowerUp"));

            if (collider)
            {
                instance.GetComponent<StateMachine>().CurrentState = new GoToPointState(collider.gameObject.transform.position);
                return;
            }

            Vector2[] vectors = _steeringBehaviour.CollisionArea(instance);

            Collider2D[] colliders = Physics2D.OverlapAreaAll(vectors[0], vectors[1], LayerMask.GetMask("Obstacle"));

            Vector2 steeringForce = Vector2.zero;
            
            if (colliders.Any())
            {
                steeringForce += _steeringBehaviour.ObstacleAvoidance(colliders);
            }
            else
            {
                steeringForce += _steeringBehaviour.Wander();
            }

            // limit to maxspeed
            steeringForce = steeringForce.normalized * instance.GetComponent<Vehicle>().MaxSpeed;

            instance.GetComponent<Rigidbody2D>().AddForce(steeringForce);

        }

        public void Enter(GameObject instance)
        {
            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.Setup(instance);

            Debug.Log("Entering Patrol State");
        }

        public void Exit(GameObject instance)
        {
            Debug.Log("Exiting Patrol State");
        }

        public void DebugDraw(GameObject instance)
        {
            Vector2[] vectors = _steeringBehaviour.CollisionArea(instance);

            Debug.DrawLine(vectors[0], vectors[1], Color.black, Time.deltaTime, false);
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
            if (msg.Msg == Message.MessageType.PowerUpDropped)
            {
                instance.GetComponent<StateMachine>().CurrentState = new GoToPointState(msg.Sender.transform.position);
            }
        }

        public void CollisionEnter(GameObject instance, Collision2D collision)
        {

        }
    }
}
