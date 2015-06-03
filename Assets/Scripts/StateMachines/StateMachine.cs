using System;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class StateMachine : MonoBehaviour
    {

        public String StartingState;

        private IState _currentState;

        void Start()
        {
            _currentState = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(typeof(StateMachine).Namespace + "." + StartingState) as IState;

            _currentState.SetGameObject(gameObject);
        }

        public IState GlobalState { get; set; }
    
        public IState CurrentState
        {
            get { return _currentState; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");

                if (CurrentState == value) return;
                PreviousState = _currentState;
                _currentState = value;

                if (PreviousState != null)
                    PreviousState.Exit(gameObject);
                CurrentState.Enter(gameObject);
            }
        }

        public IState PreviousState { get; private set; }

        void FixedUpdate()
        {
            if (GlobalState != null)
                GlobalState.Update(gameObject);
            if (CurrentState != null)
                CurrentState.Update(gameObject);
        }

        void OnGUI()
        {
            _currentState.OnGUI();
        }

        public void HandleMessage(Message msg)
        {
            CurrentState.HandleMessage(gameObject, msg);
        }

        public void Revert()
        {
            CurrentState = PreviousState;
        }
    }
}
