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
            if (flag.GetComponent<Flag>().Side == Instance.GetComponent<Tank.Tank>().Side)
                return 0;

            Module module = new Module();

            Variable distToTarget = module.CreateFLV("DistToFlag");

            Set close = distToTarget.Add("Flag_Close", new LeftShoulder(0, 0, 200));
            Set far = distToTarget.Add("Flag_Far", new RightShoulder(0, 200, 200));

            Variable enemyAllyRatio = module.CreateFLV("AllyCount");
            Set enemies = enemyAllyRatio.Add("Enemies", new LeftShoulder(-15, -2, 0));
            Set neutral = enemyAllyRatio.Add("Neutral", new Triangle(-2, 0, 2f));
            Set allies = enemyAllyRatio.Add("Allies", new RightShoulder(0, 2, 15));

            Variable desirability = module.CreateFLV("Desirability");
            Set desirable = desirability.Add("Desirable", new RightShoulder(0, 95, 95));
            Set undesirable = desirability.Add("Undesirable", new LeftShoulder(0, 0, 95));

            float dist = Vector2.Distance(Instance.transform.position, flag.transform.position);

            module["DistToFlag"].Fuzzify(dist);

            Collider2D[] tanks = Physics2D.OverlapCircleAll(flag.transform.position, 3f, LayerMask.GetMask("Tank"));

            int ratio = 0;
            foreach (Collider2D tank in tanks)
                ratio += (tank.GetComponent<Tank.Tank>().Side == Instance.GetComponent<Tank.Tank>().Side) ? 1 : -1;

            module["AllyCount"].Fuzzify(ratio);

            // close && enemies seems like a dumb move and it actually is, but otherwise the game would be very boring since they will avoid each other
            module.Add(close && (enemies || neutral), desirable);
            module.Add(close && allies, desirable.Fairly());

            module.Add(far, undesirable.Very());
            module.Add(far && neutral, desirable.Fairly());

            float crisp = module.Defuzzify("Desirability");

            return crisp;
        }

        public override Goal GetGoal()
        {
            return new Goals.CaptureFlag(_flag);
        }
    }
}
