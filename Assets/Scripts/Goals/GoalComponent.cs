using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Goals.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class GoalComponent : MonoBehaviour
    {
        public Goal Think = new Think();

        void Awake()
        {
            Think.RemoveAllSubGoals();

            Think.SetGameObject(gameObject);
            Think.Activate();
        }

        void Update()
        {
            Think.Process();
        }
    }
}
