using Assets.Scripts.Tank;
using UnityEngine;

namespace Assets.Scripts.Goals
{
    class Attack : Goal
    {
        public readonly GameObject Target;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public Attack(GameObject target)
        {
            Target = target;
        }

        public override void Activate()
        {
            Instance.GetComponentInChildren<ChatBubble>().Text = "Attacking " + Target.name;

            _steeringBehaviour = new SteeringBehaviour(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

        }

        public override STATUS Process()
        {
            if (Target == null)
                return SetStatus(STATUS.Completed);

            Vector2 steeringForce = Vector2.zero;

            if (Vector2.Distance(Instance.transform.position, Target.transform.position) < 2f)
            {
                steeringForce += _steeringBehaviour.Stop(0.2f);
            }
            else
            {
                steeringForce += _steeringBehaviour.Pursuit(Target.transform.position, Target.GetComponent<Rigidbody2D>().velocity);
            }
            
            steeringForce += _steeringBehaviour.ObstacleAvoidance(Physics2D.OverlapCircleAll(Instance.transform.position, 10f, LayerMask.GetMask("Obstacle")));

            _rigidbody.AddForce(steeringForce);

            return SetStatus(STATUS.Active);
        }

        public override void Terminate()
        {
            
        }

        public override bool IsSameGoal(Goal goal)
        {
            if (goal is Attack)
            {
                return Target == ((Attack) goal).Target;
            }

            return base.IsSameGoal(goal);
        }
    }
}
