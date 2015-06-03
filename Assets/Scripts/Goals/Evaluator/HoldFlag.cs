using Assets.Scripts.Goals.Fuzzy;
using Assets.Scripts.Goals.Fuzzy.Operator;
using Assets.Scripts.Goals.Fuzzy.Set;
using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals.Evaluator
{
    class HoldFlag : GoalEvaluator
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
            // if the flag isnt ours, we can't even hold it
            if (flag.GetComponent<Flag>().Side != Instance.GetComponent<global::Assets.Scripts.Tank.Tank>().Side)
                return 0;

            Module module = new Module();

            Variable numOfAlliesAtFlag = module.CreateFLV("HoldFlag");
            Set noAllies = numOfAlliesAtFlag.Add("Allies_None", new RightShoulder(10, 14, 15));
            Set someAllies = numOfAlliesAtFlag.Add("Allies_Some", new LeftShoulder(0, 10, 14));

            Variable desirability = module.CreateFLV("Desirability");
            Set desirable = desirability.Add("Undesirable", new RightShoulder(80, 90, 100));
            Set undesirable = desirability.Add("Desirable", new LeftShoulder(0, 80, 90));

            module.Add(noAllies, desirable);
            module.Add(someAllies, undesirable);

            Collider2D[] tanks = Physics2D.OverlapCircleAll(flag.transform.position, flag.GetComponent<Flag>().CappingRange,
                LayerMask.GetMask("Tank"));

            int allies = 0;

            foreach (Collider2D tank in tanks)
                if (tank.GetComponent<global::Assets.Scripts.Tank.Tank>().Side == Instance.GetComponent<global::Assets.Scripts.Tank.Tank>().Side)
                    allies++;

            module["HoldFlag"].Fuzzify(15 - allies);

            float crisp = module.Defuzzify("Desirability");

            Debug.Log(allies + ", " + crisp);

            return crisp;
        }

        public override Goal GetGoal()
        {
            return new Goals.HoldFlag(_flag);
        }
    }
}
