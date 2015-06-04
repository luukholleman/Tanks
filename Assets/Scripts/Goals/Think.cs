using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Goals.Evaluator;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class Think : Goal
    {
        public List<GoalEvaluator> Evaluators = new List<GoalEvaluator>();

        private float _nextEvaluation;

        public Think()
        {
            Evaluators.Add(new Evaluator.CaptureFlag());
            Evaluators.Add(new Evaluator.GetPowerUp());
            Evaluators.Add(new Evaluator.DefendFlag());
            Evaluators.Add(new Evaluator.Attack());
            Evaluators.Add(new Evaluator.Flee());
            //Evaluators.Add(new Evaluator.HoldFlag());

            _nextEvaluation = Time.timeSinceLevelLoad;
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

        }
        
        public override STATUS Process()
        {
            if (_nextEvaluation < Time.timeSinceLevelLoad)
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

                if ((SubGoals.Any() && !SubGoals.Peek().IsSameGoal(newGoal)) || !SubGoals.Any())
                {
                    RemoveAllSubGoals();
                    AddSubGoal(newGoal);
                }

                _nextEvaluation = Time.timeSinceLevelLoad + Random.value*0.2f + 0.2f;
            }

            return ProcessSubGoals();
        }

        public override void Terminate()
        {
            
        }

    }
}
