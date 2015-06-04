using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public abstract class IState
    {
        public GameObject Instance;

        public void SetGameObject(GameObject instance)
        {
            Instance = instance;
        }

        public abstract void Update(GameObject instance);
        public abstract void Enter(GameObject instance);
        public abstract void Exit(GameObject instance);

        public virtual void OnGUI() { }

        public virtual void HandleMessage(GameObject instance, Message msg) { }
    }
}
