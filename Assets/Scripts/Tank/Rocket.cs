using UnityEngine;

namespace Assets.Scripts.Tank
{
    public class Rocket : MonoBehaviour {

        private bool _exploded;

        public Player.Side Side;

        public float Damage = 0; // has to be set by tank, based on tank's power

        void Start()
        {
            Destroy(gameObject, 10f); // lifetime of 10 seconds
        }

        void Update () {
            if (_exploded)
            {
                GetComponent<SpriteRenderer>().enabled = false; // dont show after explosion
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!_exploded)
            {
                if (collider.gameObject.CompareTag("Tank") && collider.gameObject.GetComponent<Tank>().Side != Side)
                {
                    GameObject explosion = Instantiate(Resources.Load<GameObject>("PreFabs/Explosion"), transform.position, new Quaternion()) as GameObject;

                    explosion.GetComponent<Explosion>().Side = Side;

                    _exploded = true;

                    collider.GetComponent<Tank>().Health -= Damage;
                }
            }
        }
    }
}
