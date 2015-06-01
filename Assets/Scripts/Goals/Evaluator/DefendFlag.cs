using Assets.Scripts.Goals.Fuzzy;
using Assets.Scripts.Goals.Fuzzy.Operator;
using Assets.Scripts.Goals.Fuzzy.Set;
using UnityEngine;

namespace Assets.Scripts.Goals.Evaluator
{
    class DefendFlag : GoalEvaluator
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
            //if (flag.GetComponent<Flag>().Side == Player.Opposite(Instance.GetComponent<Vehicle>().Side))
            if (flag.GetComponent<Flag>().Side != Instance.GetComponent<Vehicle>().Side)
                return 0;

            Collider2D[] tanks = Physics2D.OverlapCircleAll(flag.transform.position, flag.GetComponent<Flag>().CappingRange, LayerMask.GetMask("Tank"));

            bool enemy = false;

            foreach (Collider2D tank in tanks)
                if (tank.GetComponent<Vehicle>().Side != Instance.GetComponent<Vehicle>().Side)
                {
                    enemy = true;
                    break;
                }

            if (!enemy)
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

            //// we cant defend a flag that isn't ours
            //if (flag.GetComponent<Flag>().Side == Player.Opposite(Instance.GetComponent<Vehicle>().Side))
            //    return 0;

            //Module module = new Module();

            //Variable numOfAlliesAtFlag = module.CreateFLV("DefendFlag");
            //Set noEnemies = numOfAlliesAtFlag.Add("Enemies_None", new LeftShoulder(0, 0, 15));
            //Set someEnemies = numOfAlliesAtFlag.Add("Enemies_Some", new RightShoulder(0, 15, 15));

            //Variable desirability = module.CreateFLV("Desirability");
            //Set desirable = desirability.Add("Undesirable", new RightShoulder(25, 75, 100));
            //Set undesirable = desirability.Add("Desirable", new LeftShoulder(0, 25, 75));

            //module.Add(noEnemies, desirable);
            //module.Add(someEnemies, undesirable);

            //Collider2D[] tanks = Physics2D.OverlapCircleAll(flag.transform.position, flag.GetComponent<Flag>().CappingRange, LayerMask.GetMask("Tank"));

            //int enemies = 0;

            //foreach (Collider2D tank in tanks)
            //    if (tank.GetComponent<Vehicle>().Side != Instance.GetComponent<Vehicle>().Side)
            //        enemies++;

            //// we can't defend if there are no enemies
            //if (enemies == 0)
            //    return 0;

            //module["DefendFlag"].Fuzzify(enemies);

            //float crisp = module.Defuzzify("Desirability");

            //Debug.Log(enemies + ", " + crisp);
            
            //return crisp;
        }

        public override Goal GetGoal()
        {
            return new Goals.DefendFlag(_flag);
        }
    }
}
