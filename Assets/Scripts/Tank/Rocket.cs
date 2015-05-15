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
                DestroyImmediate(gameObject);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_exploded)
            {
                GetComponent<AudioSource>().Play();
                GameObject explosion = Instantiate(Resources.Load<GameObject>("PreFabs/Explosion"), transform.position, new Quaternion()) as GameObject;

                explosion.GetComponent<Explosion>().Side = Side;
                
                _exploded = true;
            }
        }
    }
}
