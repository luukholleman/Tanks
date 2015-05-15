using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Goals.Tank
{
    class Think : Goal
    {
        public override void Activate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Instance.transform.position, 2000f, LayerMask.GetMask("Flag"));

            GameObject closest = null;
            float dist = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                float newDist = Vector2.Distance(Instance.transform.position, collider.transform.position);
                if (newDist < dist && collider.GetComponent<Flag>().Side != Instance.GetComponent<Vehicle>().Side)
                {
                    dist = newDist;
                    closest = collider.gameObject;
                }
            }

            if (closest != null)
            {
                AddSubGoal(new CaptureFlag(closest));
                return;
            }
        }
        
        public override STATUS Process()
        {
            if (!SubGoals.Any())
            {
                Activate();
            }

            if (SubGoals.Peek().GetType() != typeof(GetPowerUp))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(Instance.transform.position, 10f, LayerMask.GetMask("PowerUp"));

                GameObject closest = null;
                float dist = float.MaxValue;

                foreach (Collider2D collider in colliders)
                {
                    float newDist = Vector2.Distance(Instance.transform.position, collider.transform.position);
                    if (newDist < dist)
                    {
                        dist = newDist;
                        closest = collider.gameObject;
                    }
                }

                if (closest != null)
                {
                    AddSubGoal(new GetPowerUp(closest));
                }
            }

            return ProcessSubGoals();
        }

        public override void Terminate()
        {
            
        }

        public override bool HandleMessage()
        {
            return true;
        }
    }
}
