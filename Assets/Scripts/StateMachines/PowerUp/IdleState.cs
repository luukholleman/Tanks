using Assets.Scripts.Pathfinding;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines.PowerUp
{
    public class IdleState : IState
    {

        public override void Update(GameObject instance)
        {

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
                int val = (int)(Random.value * 10);

                GameObject powerUp;

                if (val <= 3)
                    powerUp = GameObject.Instantiate(Resources.Load<GameObject>("PreFabs/PowerUps/Movement"), msg.Sender.transform.position, new Quaternion()) as GameObject;
                else if (val <= 6)
                    powerUp = GameObject.Instantiate(Resources.Load<GameObject>("PreFabs/PowerUps/Reload"), msg.Sender.transform.position, new Quaternion()) as GameObject;
                else if (val <= 9)
                    powerUp = GameObject.Instantiate(Resources.Load<GameObject>("PreFabs/PowerUps/Repair"), msg.Sender.transform.position, new Quaternion()) as GameObject;
                else
                    powerUp = GameObject.Instantiate(Resources.Load<GameObject>("PreFabs/PowerUps/ExtraTank"), msg.Sender.transform.position, new Quaternion()) as GameObject;

                powerUp.transform.parent = GameObject.Find("PowerUps").transform;
            }
        }
    }
}
