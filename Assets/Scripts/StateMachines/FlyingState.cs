using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    class FlyingState : IState
    {
        private SteeringBehaviour _steeringBehaviour;
        public void Update(GameObject instance)
        {
            Vector2 steeringForce = Vector2.zero;

            steeringForce += RocketEvading(instance) * 50;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(instance.transform.position, 1, LayerMask.GetMask("Robot"));

            List<GameObject> neighbours = new List<GameObject>();

            foreach (Collider2D collider2D in colliders)
            {
                if (collider2D.gameObject != instance.gameObject)
                    neighbours.Add(collider2D.gameObject);
            }

            steeringForce += _steeringBehaviour.Alignment(neighbours) * instance.transform.GetComponent<Vehicle>().MaxSpeed;
            steeringForce += _steeringBehaviour.Cohesion(neighbours) * instance.transform.GetComponent<Vehicle>().MaxSpeed;
            steeringForce += _steeringBehaviour.Separation(neighbours) * instance.transform.GetComponent<Vehicle>().MaxSpeed;
            steeringForce += _steeringBehaviour.Wander();

            steeringForce = steeringForce.normalized * instance.transform.GetComponent<Vehicle>().MaxSpeed;
            
            instance.GetComponent<Rigidbody2D>().AddForce(steeringForce);
        }

        public Vector2 RocketEvading(GameObject instance)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(instance.transform.position, 10, LayerMask.GetMask("Rocket"));

            //List<GameObject> rocketsGos = new List<GameObject>();

            Vector2 steeringForce = Vector2.zero;

            foreach (Collider2D collider2D in colliders)
                steeringForce += _steeringBehaviour.Flee(collider2D.gameObject.transform.position);

            return steeringForce;
        }

        public void Enter(GameObject instance)
        {
            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(instance);
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
