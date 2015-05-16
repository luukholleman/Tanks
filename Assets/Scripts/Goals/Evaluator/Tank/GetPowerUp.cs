using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Goals.Evaluator.Tank
{
    class GetPowerUp : GoalEvaluator
    {
        private GameObject _powerUp; // we need to store the powerUp for which we calculated the score for



        public GetPowerUp()
        {

        }

        public override float CalculateDesirability()
        {
            float score = float.MinValue;

            // Loop all flags to calculate their respective score, return the best powerUp
            foreach (Transform powerUp in GameObject.Find("PowerUps").transform)
            {
                float newScore = CalculateDesirabilityForPowerUp(powerUp.gameObject);

                if (newScore > score)
                {
                    score = newScore;
                    _powerUp = powerUp.gameObject;
                }
            }

            return score;
        }

        public float CalculateDesirabilityForPowerUp(GameObject powerUp)
        {

            float score = 0;

            Transform closest = null;
            float dist = float.MaxValue;

            foreach (Transform tank in GameObject.Find("Tanks").transform)
            {
                float newDist = Vector2.Distance(powerUp.transform.position, tank.position);

                if (newDist < dist)
                {
                    dist = newDist;
                    closest = tank;
                }
            }

            // if we're the closest, get it
            if (closest == Instance.transform)
            {
                score = 100;
            }

            return score;
        }

        public override Goal GetGoal()
        {
            return new Goals.Tank.GetPowerUp(_powerUp);
        }
    }
}
