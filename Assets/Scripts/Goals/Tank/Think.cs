using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                AddSubGoal(newGoal);
            }

            //if (SubGoals.Any() && SubGoals.Peek().GetType() != typeof(GetPowerUp))
            //{
            //    Collider2D[] powerUps = Physics2D.OverlapCircleAll(Instance.transform.position, 10f, LayerMask.GetMask("PowerUp"));

            //    GameObject closestPowerup = null;
            //    float dist = float.MaxValue;

            //    foreach (Collider2D collider in powerUps)
            //    {
            //        float newDist = Vector2.Distance(Instance.transform.position, collider.transform.position);
            //        if (newDist < dist)
            //        {
            //            dist = newDist;
            //            closestPowerup = collider.gameObject;
            //        }
            //    }

            //    if (closestPowerup != null)
            //    {
            //        AddSubGoal(new GetPowerUp(closestPowerup));
            //    }
            //}

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
