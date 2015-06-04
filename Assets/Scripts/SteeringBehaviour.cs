using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    class SteeringBehaviour
    {
        public enum Deceleration
        {
            Slow = 3,
            Normal = 2,
            Fast = 1
        };

        public static float MinBoxLength = 2f;

        public GameObject Instance;

        private readonly Rigidbody2D _rigidbody;

        private readonly Tank.Tank _tank;

        private Vector2 _wanderTarget = new Vector2(0, 0);

        public SteeringBehaviour(GameObject instance)
        {
            Instance = instance;

            _rigidbody = Instance.GetComponent<Rigidbody2D>();

            _tank = Instance.GetComponent<Tank.Tank>();
        }

        public Vector2 Seek(Vector2 targetPos)
        {
            Vector2 desiredVelocity = (targetPos - (Vector2)Instance.transform.position).normalized *
                                      _tank.MaxSpeed;

            return (desiredVelocity - _rigidbody.velocity);
        }

        public Vector2 Flee(Vector2 toEvade)
        {
            Vector2 desiredVelocity = ((Vector2)Instance.transform.position - toEvade).normalized *
                                      _tank.MaxSpeed;

            return desiredVelocity - _rigidbody.velocity;
        }

        public Vector2 Arrive(Vector2 targetPos, Deceleration deceleration)
        {
            Vector2 toTarget = targetPos - (Vector2)Instance.transform.position;

            float distance = toTarget.magnitude;

            if (distance > 0)
            {
                float decelerationTweaker = 0.3f;

                float speed = distance / ((int)deceleration * decelerationTweaker);

                speed = Math.Min(speed, _tank.MaxSpeed);

                Vector2 desiredVelocity = toTarget * speed / distance;

                return desiredVelocity - _rigidbody.velocity;
            }

            return Vector2.zero;
        }

        public Vector2 Wander()
        {
            const float wanderRadius = 2;

            const float wanderDistance = 4;

            const float wanderJitter = 0.5f;

            _wanderTarget += new Vector2((Random.value * 2f - 1) * wanderJitter, (Random.value * 2f - 1) * wanderJitter);

            _wanderTarget.Normalize();

            _wanderTarget *= wanderRadius;

            Vector2 targetLocal = _wanderTarget + new Vector2(0, wanderDistance);

            Vector2 targetWorld = Instance.transform.TransformPoint(targetLocal);
            
            return Seek(targetWorld);
        }

        public Vector2[] CollisionArea()
        {
            float mag = _rigidbody.velocity.magnitude;

            float maxSpeed = _tank.MaxSpeed;

            float y = MinBoxLength +
                      (mag / maxSpeed) *
                      MinBoxLength;
            
            Vector2[] vectors = new Vector2[2];

            vectors[0] = Instance.transform.TransformPoint(new Vector2(-0.8f, -0.4f));
            vectors[1] = Instance.transform.TransformPoint(new Vector2(0.8f, y));

            return vectors;
        }

        public Vector2 ObstacleAvoidance(Collider2D[] colliders)
        {
            GameObject closestObstacle = null;

            const float maxFloat = float.MaxValue;

            float distClosestIp = maxFloat;

            Vector2 localPosOfClosestObstacle = Vector2.zero;

            foreach (Collider2D collider in colliders)
            {
                Vector2 localPos = Instance.transform.InverseTransformPoint(collider.gameObject.transform.position);

                if (localPos.y >= 0)
                {
                    if (collider.gameObject.GetComponent<CircleCollider2D>() == null)
                        continue;

                    float expandedRadius = collider.gameObject.GetComponent<CircleCollider2D>().radius + Instance.GetComponent<BoxCollider2D>().size.x / 2;

                    if (Mathf.Abs(localPos.x) < expandedRadius)
                    {
                        float cX = localPos.x;
                        float cY = localPos.y;

                        float sqrtPart = (expandedRadius * expandedRadius - cX * cX);

                        float ip = cY - sqrtPart;

                        if (ip <= 0)
                        {
                            ip = cY + sqrtPart;
                        }

                        if (ip < distClosestIp)
                        {
                            distClosestIp = ip;

                            closestObstacle = collider.gameObject;

                            localPosOfClosestObstacle = localPos;
                        }
                    }
                }
            }

            Vector2 steeringForce = Vector2.zero;

            if (closestObstacle)
            {
                float multiplier = 2f + (MinBoxLength - localPosOfClosestObstacle.y) / MinBoxLength;

                steeringForce.x = (closestObstacle.GetComponent<CircleCollider2D>().radius - localPosOfClosestObstacle.x) *
                                  multiplier;

                float brakingWeight = 0.2f;

                steeringForce.y = (closestObstacle.GetComponent<CircleCollider2D>().radius - localPosOfClosestObstacle.y) *
                                  brakingWeight;
            }

            // transform to worldvector velocity
            steeringForce = Instance.transform.TransformVector(steeringForce);

            return steeringForce;
        }

        public Vector2 Separation(List<GameObject> neighbours)
        {
            Vector2 steeringForce = Vector2.zero;

            foreach (GameObject gameObject in neighbours)
            {
                Vector2 toAgent = Instance.transform.position - gameObject.transform.position;

                //scale the force inversely proportional to the agents distance  
                //from its neighbor.
                steeringForce += toAgent.normalized / toAgent.magnitude;
            }

            return steeringForce;
        }


        public Vector2 Alignment(List<GameObject> neighbours)
        {
            //used to record the average heading of the neighbors
            Vector2 AverageHeading = Vector2.zero;

            //used to count the number of vehicles in the neighborhood
            int NeighborCount = 0;

            foreach (GameObject gameObject in neighbours)
            {
                AverageHeading += gameObject.GetComponent<Rigidbody2D>().velocity;

                ++NeighborCount;
            }

            //if the neighborhood contained one or more vehicles, average their
            //heading vectors.
            if (NeighborCount > 0)
            {
                AverageHeading /= NeighborCount;

                AverageHeading -= Instance.GetComponent<Rigidbody2D>().velocity;
            }

            return AverageHeading;
        }

        public Vector2 Cohesion(List<GameObject> neighbours)
        {
            //first find the center of mass of all the agents
            Vector2 CenterOfMass = Vector2.zero;
            Vector2 SteeringForce = Vector2.zero;

            int NeighborCount = 0;

            foreach (GameObject gameObject in neighbours)
            {
                CenterOfMass += (Vector2)gameObject.transform.position;

                ++NeighborCount;
            }

            if (NeighborCount > 0)
            {
                //the center of mass is the average of the sum of positions
                CenterOfMass /= NeighborCount;

                //now seek towards that position
                SteeringForce = Seek(CenterOfMass);
            }

            //the magnitude of cohesion is usually much larger than separation or
            //allignment so it usually helps to normalize it.
            return SteeringForce.normalized * _tank.MaxSpeed / 3;
        }

        public Vector2 Stop(float stopForce)
        {
            return _rigidbody.velocity * -1 / stopForce;
        }

        public Vector2 Pursuit(Vector2 evaderPos, Vector2 evaderVelocity)
        {
            //if the evader is ahead and facing the agent then we can just seek
            //for the evader's current position.
            Vector2 ToEvader = evaderPos - (Vector2)Instance.transform.position;

            double RelativeHeading = Vector2.Dot(_rigidbody.velocity.normalized, evaderVelocity.normalized);

            if ((Vector2.Dot(evaderVelocity.normalized, _rigidbody.velocity.normalized) > 0) && (RelativeHeading < -0.95))
            {
                return Seek(evaderPos);
            }

            //Not considered ahead so we predict where the evader will be.

            //the lookahead time is propotional to the distance between the evader
            //and the pursuer; and is inversely proportional to the sum of the
            //agent's velocities
            float LookAheadTime = evaderVelocity.magnitude / (_tank.MaxSpeed + evaderVelocity.magnitude);

            //now seek to the predicted future position of the evader
            return Seek(evaderPos + evaderVelocity * LookAheadTime);
        }

        public Vector2 Orbit(Vector2 center)
        {
            // if you set your velocity towards the center at the same speed as you're moving forward, you'll move in circles

            Vector2 local = Instance.transform.InverseTransformPoint(center);

            Vector2 localVelocity = Instance.transform.InverseTransformVector(_rigidbody.velocity);

            Vector2 localForce = new Vector2(localVelocity.y * local.normalized.x, 0);

            return Instance.transform.TransformVector(localForce).normalized * _tank.MaxSpeed;
        }
    }
}
