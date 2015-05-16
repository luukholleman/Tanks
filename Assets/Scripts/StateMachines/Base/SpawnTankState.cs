using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines.Base
{
    class SpawnTankState : IState
    {
        public void Update(GameObject instance)
        {
        }

        public void Enter(GameObject instance)
        {
            GameObject tank = Resources.Load<GameObject>("PreFabs/Tank");

            GameObject newTank = GameObject.Instantiate(tank, instance.transform.position, new Quaternion()) as GameObject;

            newTank.transform.parent = GameObject.Find("Tanks").transform;
            newTank.GetComponent<Vehicle>().Side = instance.GetComponent<Spawn>().Side;

            //newTank.GetComponent<StateMachine>().CurrentState = new PatrolState();

            instance.GetComponent<StateMachine>().CurrentState = new IdleState();
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
