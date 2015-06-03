using UnityEngine;

namespace Assets.Scripts.Tank
{
    public class Rocket : MonoBehaviour {

        private bool _exploded = false;

        public Player.Side Side;

        public float Damage = 0; // has to be set by tank, based on tank's power

        void Start()
        {
            Destroy(gameObject, 10f);
        }

        // Update is called once per frame
        void Update () {
            //if (_exploded && !GetComponent<AudioSource>().isPlaying)
            if (_exploded)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!_exploded)
            {
                if (collider.gameObject.CompareTag("Tank") && collider.gameObject.GetComponent<global::Assets.Scripts.Tank.Tank>().Side != Side)
                {
                    //GetComponent<AudioSource>().Play();
                    GameObject explosion = Instantiate(Resources.Load<GameObject>("PreFabs/Explosion"), transform.position, new Quaternion()) as GameObject;

                    explosion.GetComponent<Explosion>().Side = Side;

                    _exploded = true;

                    collider.GetComponent<global::Assets.Scripts.Tank.Tank>().Health -= Damage;

                    //Vector2 force = transform.InverseTransformPoint(collider.transform.position);

                    //collider.GetComponent<Rigidbody2D>().AddForceAtPosition(-force * 500, transform.position);
                }
            }
        }
    }
}
