using System;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class StateMachine : MonoBehaviour {
        private IState _currentState;

        private GameObject _debugText;

        void Start()
        {

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

            if (Settings.DebugState)
            {
                if (_debugText == null)
                {
                    _debugText = Instantiate(Resources.Load<GameObject>("PreFabs/Text"));
                }

                _debugText.transform.position = transform.position;
                _debugText.GetComponent<TextMesh>().text = CurrentState.GetType().Name;

                CurrentState.DebugDraw(gameObject);
            }
            else if(_debugText != null)
            {
                Destroy(_debugText);
                _debugText = null;
            }

        }

        public void HandleMessage(Message msg)
        {
            CurrentState.HandleMessage(gameObject, msg);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            CurrentState.CollisionEnter(gameObject, collision);
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            CurrentState.CollisionExit(gameObject, collision);
        }

        public void OnCollisionStay2D(Collision2D collision)
        {
            CurrentState.CollisionStay(gameObject, collision);
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            CurrentState.TriggerEnter(gameObject, collider);
        }

        public void OnTriggerExit2D(Collider2D collider)
        {
            CurrentState.TriggerExit(gameObject, collider);
        }

        public void OnTriggerStay2D(Collider2D collider)
        {
            CurrentState.TriggerStay(gameObject, collider);
        }

        public void Revert()
        {
            CurrentState = PreviousState;
        }
    }
}
