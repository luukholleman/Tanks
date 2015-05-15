using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private bool _exploded = false;

	// Use this for initialization
	void Start () {
	
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
            Instantiate(Resources.Load<GameObject>("PreFabs/Explosion"), transform.position, new Quaternion());

            if (collision.gameObject.CompareTag("Robot"))
            {
                Destroy(collision.gameObject, 0.1f);
            }

            _exploded = true;
        }
    }
}
