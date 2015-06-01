using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class GetPowerUp : Goal
    {
        private GameObject _powerUp;

        public GetPowerUp(GameObject powerUp)
        {
            _powerUp = powerUp;
        }

        public override void Activate()
        {
            Instance.GetComponentInChildren<ChatBubble>().Text = "Getting this powerup";

            if (_powerUp == null)
            {
                SetStatus(STATUS.FAILED);
                return;
            }   

            AddSubGoal(new FollowPath(_powerUp.transform.position));
        }

        public override STATUS Process()
        {
            if (_powerUp == null)
            {
                return SetStatus(STATUS.FAILED);
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

        public override int CompareTo(object obj)
        {
            return ((GetPowerUp)obj)._powerUp == _powerUp ? 0 : 1;
        }
    }
}
