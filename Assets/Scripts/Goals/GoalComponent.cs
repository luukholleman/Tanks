using UnityEngine;

namespace Assets.Scripts.Goals
{
    class GoalComponent : MonoBehaviour
    {
        public Goal Think = new Think();

        void Start()
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
