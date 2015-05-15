using UnityEngine;
using System.Collections;

public class LaserShooter : MonoBehaviour {


    private float lastShoot = 0;

    public float ReloadTime = 1;

    private LineRenderer line;

    // Use this for initialization
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(2);
        line.SetWidth(0.05f, 0.05f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 40, LayerMask.GetMask("Rocket", "Tank"));

        GameObject closestGameObject = null;
        float dist = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            Vector2 localPos = transform.TransformPoint(collider.transform.position);

            if (localPos.magnitude - Random.value < dist)
                { 
                    Vector2 heading = collider.transform.position - transform.position;

                    RaycastHit2D hit = Physics2D.Raycast(transform.position, heading / heading.magnitude, float.MaxValue, LayerMask.GetMask("Obstacle", "Rocket", "Tank"));

                    if (hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject == collider.gameObject)
                    {
                        closestGameObject = collider.gameObject;
	                    dist = localPos.magnitude;
                    }
	            }
        }

        if (closestGameObject != null && lastShoot + ReloadTime + Random.value / 10 < Time.time)
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, closestGameObject.transform.position);

            if (closestGameObject.CompareTag("Rocket"))
            {
                DestroyImmediate(closestGameObject);
            }

            lastShoot = Time.time;
        }
        else
        {
            line.enabled = false;
        }
    }
}
