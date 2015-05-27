using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public interface IState
    {
        void Update(GameObject instance);
        void Enter(GameObject instance);
        void Exit(GameObject instance);
        void DebugDraw(GameObject instance);
        void CollisionEnter(GameObject instance, Collision2D collision);
        void CollisionExit(GameObject instance, Collision2D collision);
        void CollisionStay(GameObject instance, Collision2D collision);
        void TriggerEnter(GameObject instance, Collider2D collider);
        void TriggerExit(GameObject instance, Collider2D collider);
        void TriggerStay(GameObject instance, Collider2D collider);
        void HandleMessage(GameObject instance, Message msg);
    }
}
