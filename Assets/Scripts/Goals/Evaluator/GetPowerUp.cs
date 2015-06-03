using Assets.Scripts.Goals.Fuzzy;
using Assets.Scripts.Goals.Fuzzy.Operator;
using Assets.Scripts.Goals.Fuzzy.Set;
using UnityEngine;

namespace Assets.Scripts.Goals.Evaluator
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
            Module module = new Module();

            Variable distToTarget = module.CreateFLV("DistToPowerup");

            Set close = distToTarget.Add("Powerup_Close", new LeftShoulder(0, 200, 200));
            Set far = distToTarget.Add("Powerup_Far", new RightShoulder(0, 200, 200));

            Variable desirability = module.CreateFLV("Desirability");
            Set desirable = desirability.Add("Desirable", new RightShoulder(0, 95, 95));
            Set undesirable = desirability.Add("Undesirable", new LeftShoulder(0, 0, 95));

            float dist = Vector2.Distance(Instance.transform.position, powerUp.transform.position);

            module["DistToPowerup"].Fuzzify(dist);

            // close && enemies seems like a dumb move and it actually is, but tanks cant know how much enemies there are
            module.Add(close, desirable.Very());

            module.Add(far, undesirable);

            float crisp = module.Defuzzify("Desirability");

            //Debug.Log(dist + ", " + ratio + ", " + crisp);

            return crisp;


            //float score = 0;

            //Transform closest = null;
            //float dist = float.MaxValue;

            //foreach (Transform tank in GameObject.Find("Tanks").transform)
            //{
            //    float newDist = Vector2.Distance(powerUp.transform.position, tank.position);

            //    if (newDist < dist)
            //    {
            //        dist = newDist;
            //        closest = tank;
            //    }
            //}

            //// if we're the closest, get it
            //if (closest == Instance.transform)
            //{
            //    score = 100;
            //}

            //return score;
        }

        public override Goal GetGoal()
        {
            return new Goals.GetPowerUp(_powerUp);
        }
    }
}
