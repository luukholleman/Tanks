using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    internal abstract class Goal
    {
        public GameObject Instance;

        public Stack<Goal> SubGoals = new Stack<Goal>(); 
        public enum STATUS {
            INACTIVE,
            ACTIVE,
            COMPLETED,
            FAILED
        }

        public STATUS Status;

        public abstract void Activate();
        public abstract STATUS Process();
        public abstract void Terminate();
        public abstract bool HandleMessage();

        public void AddSubGoal(Goal subGoal)
        {
            if(SubGoals.Any())
                SubGoals.Peek().SetStatus(STATUS.INACTIVE);

            SubGoals.Push(subGoal);

            subGoal.SetGameObject(Instance);
        }

        public virtual void SetGameObject(GameObject gameObject)
        {
            Instance = gameObject;
        }

        public STATUS ProcessSubGoals()
        {
            try
            {
                while (SubGoals.Any() && (SubGoals.Peek().Status == STATUS.COMPLETED || SubGoals.Peek().Status == STATUS.FAILED))
                {
                    Goal goal = SubGoals.Pop();

                    goal.Terminate();
                }

                if (SubGoals.Any())
                {
                    Goal subGoal = SubGoals.Peek();

                    if (subGoal.Status == STATUS.INACTIVE)
                    {
                        subGoal.Activate();
                        subGoal.Status = STATUS.ACTIVE;
                    }

                    STATUS status = subGoal.Process();

                    if (status == STATUS.COMPLETED && subGoal.SubGoals.Any())
                    {
                        return STATUS.ACTIVE;
                    }

                    return status;
                }

                return STATUS.COMPLETED;
            }
            catch (Exception)
            {
                Debug.LogWarning("Exception in ProcessSubgoals");
                return STATUS.FAILED;
            }
        }

        public void RemoveAllSubGoals()
        {
            foreach (Goal subGoal in SubGoals)
            {
                subGoal.Terminate();
            }

            SubGoals.Clear();
        }

        public STATUS SetStatus(STATUS status)
        {
            Status = status;
            return status;
        }
    }
}
