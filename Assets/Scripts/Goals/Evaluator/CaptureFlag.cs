using Assets.Scripts.Goals.Fuzzy;
using Assets.Scripts.Goals.Fuzzy.Operator;
using Assets.Scripts.Goals.Fuzzy.Set;
using UnityEngine;

namespace Assets.Scripts.Goals.Evaluator
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

            Module module = new Module();

            Variable distToTarget = module.CreateFLV("DistToFlag");

            Set close = distToTarget.Add("Flag_Close", new LeftShoulder(0, 0, 200));
            Set far = distToTarget.Add("Flag_Far", new RightShoulder(0, 200, 200));

            Variable desirability = module.CreateFLV("Desirability");

            Set desirable = desirability.Add("Desirable", new RightShoulder(0, 100, 100));
            Set undesirable = desirability.Add("Undesirable", new LeftShoulder(0, 0, 100));

            module.Add(close, desirable);
            module.Add(far, undesirable);

            float dist = Vector2.Distance(Instance.transform.position, flag.transform.position);

            module["DistToFlag"].Fuzzify(dist);

            float crisp = module.Defuzzify("Desirability");

            return crisp;
        }

        public override Goal GetGoal()
        {
            return new Goals.CaptureFlag(_flag);
        }
    }
}
