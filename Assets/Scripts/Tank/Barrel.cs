using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tank;

public class Barrel : MonoBehaviour {
    
    private float _lastShot = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20, LayerMask.GetMask("Robot", "Tank"));

        GameObject closestGameObject = null;
        float dist = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject.GetComponent<Vehicle>().Side == GetComponentInParent<Vehicle>().Side)
                continue;

            if (Vector2.Distance(collider.gameObject.transform.position, transform.position) < dist)
            {
                Vector2 heading = collider.transform.position - transform.position;

                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + heading.normalized, heading, float.MaxValue, LayerMask.GetMask("Obstacle", "Robot", "Tank"));

                if (hit.collider != null && hit.collider.gameObject != collider.gameObject && hit.collider.gameObject != transform.parent.gameObject && (hit.collider.gameObject.CompareTag("Robot") || hit.collider.gameObject.CompareTag("Tank")))
                {
                    if (hit.collider.GetComponent<Vehicle>().Side != GetComponentInParent<Vehicle>().Side)
                    {
                        closestGameObject = collider.gameObject;
                        dist = Vector2.Distance(collider.gameObject.transform.position, transform.position);
                    }
                }
            }
        }
        if (closestGameObject != null)
        {
            // turn barrel to object
            Vector3 direction = closestGameObject.transform.position - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            angle = angle % 360;

            angle -= 90;

            Vector3 rotNew = new Vector3(0, 0, angle);

            transform.rotation = Quaternion.Euler(rotNew);

            if (_lastShot + GetComponentInParent<Vehicle>().ReloadSpeed < Time.time)
            {
                GameObject rocket = Instantiate(Resources.Load<GameObject>("PreFabs/Rocket"), transform.position + transform.TransformVector(new Vector3(0, 1, 0)), transform.rotation) as GameObject;

                rocket.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, 5000));

                rocket.GetComponent<Rocket>().Side = GetComponentInParent<Vehicle>().Side;
                rocket.GetComponent<Rocket>().Damage = 50;

                _lastShot = Time.time;
            }
        }
	}

}
