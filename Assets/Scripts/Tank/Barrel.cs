using System.Linq;
using Assets.Scripts.Goals;
using Assets.Scripts.Tank;
using UnityEngine;

public class Barrel : MonoBehaviour {
    
    private float _lastShot;

    public float Damage = 20;

    public float RangeFromFlagMultiplier = 5;

    public float Multiplier = 1.3f;
    
    void Update()
    {
        GameObject closestGameObject = null;

        if (GetComponentInParent<GoalComponent>() != null && GetComponentInParent<GoalComponent>().Think.SubGoals.Any() && GetComponentInParent<GoalComponent>().Think.SubGoals.Peek().GetType() == typeof(Attack))
	    {
            closestGameObject = ((Attack)GetComponentInParent<GoalComponent>().Think.SubGoals.Peek()).Target;
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

            if (_lastShot + GetComponentInParent<Tank>().ReloadSpeed < Time.time)
            {
                GameObject rocket = Instantiate(Resources.Load<GameObject>("PreFabs/Rocket"), transform.position, transform.rotation) as GameObject;

                rocket.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, 5000));

                rocket.GetComponent<Rocket>().Side = GetComponentInParent<Tank>().Side;

                Collider2D[] flags = Physics2D.OverlapCircleAll(transform.position, RangeFromFlagMultiplier, LayerMask.GetMask("Flag"));
                bool inRange = false;
                foreach (Collider2D flag in flags)
                {
                    if (flag.GetComponent<Flag>().Side == GetComponentInParent<Tank>().Side)
                    {
                        inRange = true;
                        break;
                    }
                }

                rocket.GetComponent<Rocket>().Damage = Damage;
                if (inRange)
                {
                    rocket.GetComponent<Rocket>().Damage = Damage * Multiplier;
                }

                _lastShot = Time.time;
            }
        }
	}

}
