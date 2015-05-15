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
        public Goal think = new Think();

        void Start()
        {
            think.SetGameObject(gameObject);
            think.Activate();
        }

        void Update()
        {
            think.Process();
        }
    }
}
