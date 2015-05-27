using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines.Base
{
    public class IdleState : IState
    {
        private List<float> _spawnTimings = new List<float>(); 
    
        public void Update(GameObject instance)
        {
            foreach (float f in _spawnTimings.Where(t => t < Time.timeSinceLevelLoad))
            {
                GameObject tank = Resources.Load<GameObject>("PreFabs/Tank");

                GameObject newTank = GameObject.Instantiate(tank, instance.transform.position, new Quaternion()) as GameObject;

                newTank.transform.parent = GameObject.Find("Tanks").transform;
                newTank.GetComponent<Vehicle>().Side = instance.GetComponent<Spawn>().Side;
            }

            _spawnTimings.RemoveAll(t => t < Time.timeSinceLevelLoad);
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
            {
                if (msg.Sender.GetComponent<Vehicle>().Side == instance.GetComponent<Spawn>().Side)
                    _spawnTimings.Add(Time.timeSinceLevelLoad + Settings.Instance.TankSpawnDelay);
            }
        }
    }
}
