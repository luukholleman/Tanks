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

            //Debug.Log(_flag.name + " " + score);

            return score;
        }

        public float CalculateDesirabilityForFlag(GameObject flag)
        {
            if (flag.GetComponent<Flag>().Side == Instance.GetComponent<Vehicle>().Side)
                return 0;

            Module module = new Module();

            Variable distToTarget = module.CreateFLV("DistToFlag");

            Set close = distToTarget.Add("Flag_Close", new LeftShoulder(0, 200, 200));
            Set far = distToTarget.Add("Flag_Far", new RightShoulder(0, 200, 200));

            Variable enemyAllyRatio = module.CreateFLV("EnemyAllyRatio");
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
                ratio += (tank.GetComponent<Vehicle>().Side == Instance.GetComponent<Vehicle>().Side) ? 1 : -1;

            module["EnemyAllyRatio"].Fuzzify(ratio);

            module.Add(close && enemies, undesirable.Fairly());
            module.Add(close && neutral, desirable);
            module.Add(close && allies, desirable.Fairly());

            module.Add(far, undesirable.Very());
            module.Add(far && neutral, desirable.Fairly());


            float crisp = module.Defuzzify("Desirability");

            Debug.Log(dist + ", " + ratio + ", " + crisp);
            
            return crisp;


            //Module module = new Module();

            //Variable distToTarget = module.CreateFLV("DistToFlag");
            //Set close = distToTarget.Add("Flag_Close", new LeftShoulder(0, 15, 25));
            //Set far = distToTarget.Add("Flag_Far", new Triangle(15, 25, 60));
            //Set veryFar = distToTarget.Add("Flag_VeryFar", new RightShoulder(30, 60, 200));

            //Variable enemyAllyRatio = module.CreateFLV("EnemyAllyRatio");

            //Set manyEnemies = enemyAllyRatio.Add("ManyEnemies", new LeftShoulder(-15, -10, -5));
            //Set enemies = enemyAllyRatio.Add("Enemies", new Triangle(-10, -5, -1));
            //Set neutralEnemies = enemyAllyRatio.Add("NeutralEnemies", new Triangle(-5, -1, 0));
            //Set neutral = enemyAllyRatio.Add("Neutral", new Triangle(-1, 0, 1f));
            //Set neutralAllies = enemyAllyRatio.Add("NeutralAllies", new Triangle(0, 1, 5));
            //Set allies = enemyAllyRatio.Add("Allies", new Triangle(1, 5, 10));
            //Set manyAllies = enemyAllyRatio.Add("ManyAllies", new RightShoulder(5, 10, 15));

            //Variable desirability = module.CreateFLV("Desirability");
            //Set desirable = desirability.Add("Desirable", new RightShoulder(0, 100, 100));
            //Set undesirable = desirability.Add("Undesirable", new LeftShoulder(0, 0, 100));

            //float dist = Vector2.Distance(Instance.transform.position, flag.transform.position);

            //module["DistToFlag"].Fuzzify(dist);

            //Collider2D[] tanks = Physics2D.OverlapCircleAll(flag.transform.position, 3f, LayerMask.GetMask("Tank"));

            //int ratio = 0;
            //foreach (Collider2D tank in tanks)
            //    ratio += (tank.GetComponent<Vehicle>().Side == Instance.GetComponent<Vehicle>().Side) ? 1 : -1;

            //module["EnemyAllyRatio"].Fuzzify(ratio);

            //module.Add(close && manyEnemies, undesirable.Very());
            //module.Add(close && enemies, undesirable.Fairly());
            //module.Add(close && neutralEnemies, undesirable);
            //module.Add(close && neutral, undesirable);
            //module.Add(close && neutralAllies, undesirable);
            //module.Add(close && allies, undesirable.Fairly());
            //module.Add(close && manyAllies, undesirable.Very());

            //module.Add(far && manyEnemies, undesirable.Very());
            //module.Add(far && enemies, undesirable.Very());
            //module.Add(far && neutralEnemies, undesirable.Fairly());
            //module.Add(far && neutral, desirable.Fairly());
            //module.Add(far && neutralAllies, undesirable.Fairly());
            //module.Add(far && allies, undesirable.Very());
            //module.Add(far && manyAllies, undesirable.Very());

            ////module.Add(close && manyEnemies, undesirable.Very());
            ////module.Add(close && enemies, undesirable.Fairly());
            ////module.Add(close && neutralEnemies, desirable);
            ////module.Add(close && neutral, desirable);
            ////module.Add(close && neutralAllies, desirable);
            ////module.Add(close && allies, undesirable.Fairly());
            ////module.Add(close && manyAllies, undesirable.Very());

            ////module.Add(far && manyEnemies, undesirable.Very());
            ////module.Add(far && enemies, undesirable.Very());
            ////module.Add(far && neutralEnemies, undesirable.Fairly());
            ////module.Add(far && neutral, desirable.Fairly());
            ////module.Add(far && neutralAllies, undesirable.Fairly());
            ////module.Add(far && allies, undesirable.Very());
            ////module.Add(far && manyAllies, undesirable.Very());

            //module.Add(veryFar, undesirable.Very());

            //float crisp = module.Defuzzify("Desirability");

            ////Debug.Log(crisp);

            //return crisp;
        }

        public override Goal GetGoal()
        {
            return new Goals.CaptureFlag(_flag);
        }
    }
}
