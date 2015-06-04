using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class GetPowerUp : Goal
    {
        public readonly GameObject PowerUp;

        public GetPowerUp(GameObject powerUp)
        {
            PowerUp = powerUp;
        }

        public override void Activate()
        {
            Instance.GetComponentInChildren<ChatBubble>().Text = "Getting this powerup";

            if (PowerUp == null)
            {
                SetStatus(STATUS.Failed);
                return;
            }   

            AddSubGoal(new FollowPath(PowerUp.transform.position));
        }

        public override STATUS Process()
        {
            if (PowerUp == null)
            {
                return SetStatus(STATUS.Failed);
            }

            return ProcessSubGoals();   
        }

        public override void Terminate()
        {
            
        }

        public override bool IsSameGoal(Goal goal)
        {
            if (goal is GetPowerUp)
            {
                return PowerUp == ((GetPowerUp)goal).PowerUp;
            }

            return base.IsSameGoal(goal);
        }
    }
}
