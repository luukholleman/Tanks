using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Goals.Evaluator;
using UnityEngine;

namespace Assets.Scripts.Goals.Tank
{
    class Think : Goal
    {
        public List<GoalEvaluator> Evaluators = new List<GoalEvaluator>();

        public Think()
        {
            Evaluators.Add(new Evaluator.Tank.CaptureFlag());
            Evaluators.Add(new Evaluator.Tank.GetPowerUp());
        }

        public override void SetGameObject(GameObject gameObject)
        {
            Instance = gameObject;

            foreach (GoalEvaluator evaluator in Evaluators)
            {
                evaluator.SetGameObject(gameObject);
            }
        }

        public override void Activate()
        {
            int rand = (int)(Random.value * 15);

            Debug.Log(rand);

            if (rand < 2)
                AddSubGoal(new CaptureFlag(GameObject.Find("Flags/Stables")));
            else if (rand < 4)
                AddSubGoal(new CaptureFlag(GameObject.Find("Flags/Farm")));
            else if (rand < 7)
                AddSubGoal(new CaptureFlag(GameObject.Find("Flags/Gold Mine")));
            else if (rand < 10)
                AddSubGoal(new CaptureFlag(GameObject.Find("Flags/Lumber Mill")));
            else if (rand < 15)
                AddSubGoal(new CaptureFlag(GameObject.Find("Flags/Blacksmith")));
        }
        
        public override STATUS Process()
        {
            GoalEvaluator bestEvaluator = null;
            float bestScore = float.MinValue;

            foreach (GoalEvaluator evaluator in Evaluators)
            {
                float score = evaluator.CalculateDesirability();

                if (score > bestScore)
                {
                    bestScore = score;
                    bestEvaluator = evaluator;
                }
            }

            Goal newGoal = bestEvaluator.GetGoal();

            if ((SubGoals.Any() && SubGoals.Peek().GetType() != newGoal.GetType()) || !SubGoals.Any())
            {
                RemoveAllSubGoals();
                AddSubGoal(newGoal);
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
