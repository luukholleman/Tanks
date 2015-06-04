using Assets.Scripts.Goals.Fuzzy;
using Assets.Scripts.Goals.Fuzzy.Operator;
using Assets.Scripts.Goals.Fuzzy.Set;
using UnityEngine;

namespace Assets.Scripts.Goals.Evaluator
{
    class Attack : GoalEvaluator
    {
        private GameObject _tank; // we need to store the tank for which we calculated the score for

        public override float CalculateDesirability()
        {
            float score = float.MinValue;

            // Loop all flags to calculate their respective score, return the best tank
            foreach (Transform tank in GameObject.Find("Tanks").transform)
            {
                float newScore = CalculateDesirabilityForTank(tank.gameObject);

                if (newScore > score)
                {
                    score = newScore;
                    _tank = tank.gameObject;
                }
            }

            return score;
        }

        public float CalculateDesirabilityForTank(GameObject tank)
        {
            if (tank.GetComponent<Tank.Tank>().Side == Instance.GetComponent<Tank.Tank>().Side)
                return 0;
            
            Module module = new Module();

            Variable distToTarget = module.CreateFLV("DistToTarget");
            Set close = distToTarget.Add("Target_Close", new LeftShoulder(0, 10, 20));
            Set far = distToTarget.Add("Target_Far", new RightShoulder(10, 20, 200));

            // health shoulders go to -50 because it can be possible to be hit by multiple rockets in 1 frame, thus reducing health below 0
            Variable currentHealth = module.CreateFLV("CurrentHealth");
            Set lowHealth = currentHealth.Add("Health_Low", new LeftShoulder(-50, 25, 50));
            Set midHealth = currentHealth.Add("Health_Middle", new Triangle(25, 50, 75));
            Set highHealth = currentHealth.Add("Health_High", new RightShoulder(50, 75, 150));

            Variable targetHealth = module.CreateFLV("TargetHealth");
            Set tarLowHealth = targetHealth.Add("Health_Low", new LeftShoulder(-50, 25, 50));
            Set tarMidHealth = targetHealth.Add("Health_Middle", new Triangle(25, 50, 75));
            Set tarHighHealth = targetHealth.Add("Health_High", new RightShoulder(50, 75, 150));

            Variable desirability = module.CreateFLV("Desirability");
            Set undesirable = desirability.Add("Undesirable", new LeftShoulder(0, 0, 100));
            Set desirable = desirability.Add("Desirable", new RightShoulder(0, 100, 100));

            float dist = Vector2.Distance(Instance.transform.position, tank.transform.position);
            float myHealth = Instance.GetComponent<Tank.Tank>().Health;
            float tarHealth = tank.GetComponent<Tank.Tank>().Health;

            module["DistToTarget"].Fuzzify(dist);
            module["CurrentHealth"].Fuzzify(myHealth);
            module["TargetHealth"].Fuzzify(tarHealth);

            module.Add(close && lowHealth && tarLowHealth, undesirable.Very());
            module.Add(close && lowHealth && tarMidHealth, undesirable);
            module.Add(close && lowHealth && tarHighHealth, undesirable.Fairly());
            module.Add(close && midHealth && tarLowHealth, desirable);
            module.Add(close && midHealth && tarMidHealth, desirable.Fairly());
            module.Add(close && midHealth && tarHighHealth, undesirable);
            module.Add(close && highHealth && tarLowHealth, desirable.Very());
            module.Add(close && highHealth && tarMidHealth, desirable.Fairly());
            module.Add(close && highHealth && tarHighHealth, desirable);

            // don't engage in far combat except when you can win for sure
            module.Add(far, undesirable.Very());
            module.Add(far && highHealth.Very() && tarLowHealth.Fairly(), desirable);
        
            float crisp = module.Defuzzify("Desirability");
            
            return crisp;
        }

        public override Goal GetGoal()
        {
            return new Goals.Attack(_tank);
        }
    }
}
