using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Goals.Tank
{
    class CaptureFlag : Goal
    {
        private GameObject _flag;

        public CaptureFlag(GameObject flag)
        {
            _flag = flag;
        }

        public override void Activate()
        {
            Debug.Log("Going to cap flag " + _flag);
            AddSubGoal(new WaitAtFlagTillCaptured(_flag));
            AddSubGoal(new FollowPath(_flag.transform.position));
        }

        public override STATUS Process()
        {
            if(_flag.GetComponent<Flag>().Side == Instance.GetComponent<Vehicle>().Side)
                return SetStatus(STATUS.FAILED);

            return ProcessSubGoals();
        }

        public override void Terminate()
        {
            Debug.Log(_flag + " is captured");
        }

        public override bool HandleMessage()
        {
            return true;
        }
    }
}
