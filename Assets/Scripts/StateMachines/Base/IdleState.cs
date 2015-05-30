using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines.Base
{
    public class IdleState : IState
    {
        private List<float> _spawnTimings = new List<float>(); 
    
        public override void Update(GameObject instance)
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

        public override void Enter(GameObject instance)
        {

        }

        public override void Exit(GameObject instance)
        {
        
        }

        public override void HandleMessage(GameObject instance, Message msg)
        {
            if (msg.Msg == Message.MessageType.TankDied)
            {
                if (msg.Sender.GetComponent<Vehicle>().Side == instance.GetComponent<Spawn>().Side)
                    _spawnTimings.Add(Time.timeSinceLevelLoad + Settings.Instance.TankSpawnDelay);
            }
        }
    }
}
