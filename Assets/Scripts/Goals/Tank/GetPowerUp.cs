using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Goals.Tank
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
    }
}
