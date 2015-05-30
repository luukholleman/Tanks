using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Pathfinding;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class Attack : Goal
    {
        public readonly GameObject Target;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public Attack(GameObject target)
        {
            Target = target;
        }

        public override void Activate()
        {
            //String msg = "ATTACKING!!! GOING TO KILL SOMEONE";
            //Messenger.BroadcastMessage(new Message(Instance, Message.MessageType.ChatMessage, msg));
            //Messenger.Dispatch();

            _steeringBehaviour = ScriptableObject.CreateInstance<SteeringBehaviour>();
            _steeringBehaviour.SetGameObject(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

        }

        public override STATUS Process()
        {
            if (Target == null)
                return SetStatus(STATUS.COMPLETED);

            Vector2 steeringForce = Vector2.zero;

            steeringForce += _steeringBehaviour.Pursuit(Target.transform.position, Target.GetComponent<Rigidbody2D>().velocity);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(Instance.transform.position, 2f, LayerMask.GetMask("Tank"));
            List<GameObject> neighbours = new List<GameObject>();

            foreach (Collider2D collider2D in colliders) 
                if (collider2D.gameObject != Instance.gameObject) neighbours.Add(collider2D.gameObject);

            steeringForce += _steeringBehaviour.Separation(neighbours);

            steeringForce = steeringForce.normalized * Instance.transform.GetComponent<Vehicle>().MaxSpeed;

            _rigidbody.AddForce(steeringForce);

            return SetStatus(STATUS.ACTIVE);
        }

        public override void Terminate()
        {
            
        }

        public override bool HandleMessage()
        {
            return true;
        }
    }
}
