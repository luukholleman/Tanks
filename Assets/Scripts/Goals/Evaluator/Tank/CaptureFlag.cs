using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Goals.Evaluator.Tank
{
    class CaptureFlag : GoalEvaluator
    {
        private GameObject _flag; // we need to store the flag for which we calculated the score for

        public override float CalculateDesirability()
        {
            float score = float.MinValue;

            // Loop all flags to calculate their respective score, return the best flag
            foreach (Transform flag in GameObject.Find("Flags").transform)
            {
                float newScore = CalculateDesirabilityForFlag(flag.gameObject);

                if (newScore > score)
                {
                    score = newScore;
                    _flag = flag.gameObject;
                }
            }

            return score;
        }

        public float CalculateDesirabilityForFlag(GameObject flag)
        {
            if (flag.GetComponent<Flag>().Side == Instance.GetComponent<Vehicle>().Side)
                return 0;

            float score = 0;

            score += 100 - Vector2.Distance(Instance.transform.position, flag.transform.position);

            return score;
        }

        public override Goal GetGoal()
        {
            return new Goals.Tank.CaptureFlag(_flag);
        }
    }
}
