using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.Tank
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Tank : MonoBehaviour
    {
        public float MaxSpeed = 4;

        public float ReloadSpeed = 1;

        public float Health = 100;

        public Player.Side Side;
        
        void Start()
        {
            name = new Namer().Name;

            if (Side == Player.Side.Blue)
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/tankBlue_outline");

                foreach (Transform t in transform)
                {
                    if (t.name == "Barrel")
                        t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/barrelBlue_outline");
                }
            }
            else if (Side == Player.Side.Red)
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/tankRed_outline");

                foreach (Transform t in transform)
                {
                    if (t.name == "Barrel")
                        t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/barrelRed_outline");
                }
            }
        }
    
        void Update()
        {
            if (Health <= 0)
            {
                Message msg = new Message(gameObject, Message.MessageType.TankDied);

                Messenger.BroadcastMessage(msg);

                Messenger.Dispatch();

                gameObject.SetActive(false);

                Destroy(gameObject);
            }
        }

        void LateUpdate()
        {
            Vector3 moveDirection = GetComponent<Rigidbody2D>().velocity;

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            angle -= 90;

            angle = angle % 360;

            Vector3 rotNew = new Vector3(0, 0, angle);

            transform.rotation = Quaternion.Euler(rotNew);
        }
    }
}
