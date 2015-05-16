using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    class SteeringBehaviour : ScriptableObject
    {
        public enum Deceleration
        {
            slow = 3,
            normal = 2,
            fast = 1
        };

        public static float MinBoxLength = 2f;

        private GameObject _instance;

        private Rigidbody2D _rigidbody2D;

        private Vehicle _vehicle;

        private Vector2 wanderTarget = new Vector2(0, 0);

        public void SetGameObject(GameObject instance)
        {
            _instance = instance;

            _rigidbody2D = _instance.GetComponent<Rigidbody2D>();

            _vehicle = _instance.GetComponent<Vehicle>();
        }

        public Vector2 Seek(Vector2 targetPos)
        {
            Vector2 desiredVelocity = (targetPos - (Vector2)_instance.transform.position).normalized *
                                      _vehicle.MaxSpeed;

            return (desiredVelocity - _rigidbody2D.velocity);
        }

        public Vector2 Flee(Vector2 targetPos)
        {
            Vector2 desiredVelocity = ((Vector2)_instance.transform.position - targetPos).normalized *
                                      _vehicle.MaxSpeed;

            return desiredVelocity - _rigidbody2D.velocity;
        }

        public Vector2 Arrive(Vector2 targetPos, Deceleration deceleration)
        {
            Vector2 toTarget = targetPos - (Vector2)_instance.transform.position;

            float distance = toTarget.magnitude;

            if (distance > 0)
            {
                float decelerationTweaker = 0.3f;

                float speed = distance / ((int)deceleration * decelerationTweaker);

                speed = Math.Min(speed, _vehicle.MaxSpeed);

                Vector2 desiredVelocity = toTarget * speed / distance;

                return desiredVelocity - _rigidbody2D.velocity;
            }

            return Vector2.zero;
        }

        public Vector2 Wander()
        {
            const float wanderRadius = 2;

            const float wanderDistance = 4;

            const float wanderJitter = 0.5f;

            wanderTarget += new Vector2((Random.value * 2f - 1) * wanderJitter, (Random.value * 2f - 1) * wanderJitter);

            wanderTarget.Normalize();

            wanderTarget *= wanderRadius;

            Vector2 targetLocal = wanderTarget + new Vector2(0, wanderDistance);

            Vector2 targetWorld = _instance.transform.TransformPoint(targetLocal);

            if (Settings.DebugState)
            {
                float theta_scale = 0.1f;             //Set lower to add more points
                float size = (2.0f * (float)Math.PI) / theta_scale; //Total number of points in circle.

                //LineRenderer lineRenderer = _instance.GetComponent<LineRenderer>();
                //if (lineRenderer == null)
                //    lineRenderer = _instance.AddComponent<LineRenderer>();

                ////lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
                //lineRenderer.SetColors(Color.black, Color.red);
                //lineRenderer.SetWidth(0.05F, 0.05F);
                //lineRenderer.SetVertexCount((int)size + 1);

                int i = 0;
                for (float theta = 0; theta < 2 * Math.PI; theta += 0.1f)
                {
                    float x = wanderRadius * (float)Math.Cos(theta);
                    float y = wanderRadius * (float)Math.Sin(theta);

                    Vector3 pos = new Vector3(x, y, 0);

                    pos.y += wanderDistance;

                    //lineRenderer.SetPosition(i, _instance.transform.TransformPoint(pos));
                    i += 1;
                }

                Debug.DrawLine(_instance.transform.position, targetWorld);
            }

            return Seek(targetWorld);
        }

        public Vector2[] CollisionArea(GameObject instance)
        {
            float mag = instance.GetComponent<Rigidbody2D>().velocity.magnitude;

            float maxSpeed = instance.GetComponent<Vehicle>().MaxSpeed;

            float y = MinBoxLength +
                      (mag / maxSpeed) *
                      MinBoxLength;
            
            Vector2[] vectors = new Vector2[2];

            vectors[0] = instance.transform.TransformPoint(new Vector2(-0.8f, -0.4f));
            vectors[1] = instance.transform.TransformPoint(new Vector2(0.8f, y));

            return vectors;
        }

        public Vector2 ObstaclesAvoidance(Collider2D[] colliders)
        {

            GameObject closestObstacle = null;

            const float maxFloat = float.MaxValue;

            float distClosestIp = maxFloat;

            List<GameObject> gos = new List<GameObject>();

            foreach (Collider2D collider in colliders)
            {
                Vector2 localPos = _instance.transform.InverseTransformPoint(collider.gameObject.transform.position);

                if (localPos.y >= 0)
                {
                    if (collider.gameObject.GetComponent<CircleCollider2D>() == null)
                        continue;

                    float expandedRadius = collider.gameObject.GetComponent<CircleCollider2D>().radius + _instance.GetComponent<BoxCollider2D>().size.x / 2;

                    if (Mathf.Abs(localPos.x) < expandedRadius)
                    {
                        gos.Add(collider.gameObject);
                    }
                }
            }

            Vector2 steeringForce = Vector2.zero;

            foreach (GameObject gameObject in gos)
            {
                Vector2 localPosOfClosestObstacle = _instance.transform.TransformPoint(gameObject.transform.position);

                float multiplier = 2f + (MinBoxLength - localPosOfClosestObstacle.y) / MinBoxLength;

                steeringForce.x += (gameObject.GetComponent<CircleCollider2D>().radius - localPosOfClosestObstacle.x) *
                                  multiplier;

                float brakingWeight = 0.2f;

                steeringForce.y += (gameObject.GetComponent<CircleCollider2D>().radius - localPosOfClosestObstacle.y) *
                                  brakingWeight;
            }


            // transform to worldvector velocity
            steeringForce = _instance.transform.TransformVector(steeringForce);

            steeringForce.Normalize();

            return steeringForce;
        }

        public Vector2 ObstacleAvoidance(Collider2D[] colliders)
        {
            GameObject closestObstacle = null;

            const float maxFloat = float.MaxValue;

            float distClosestIp = maxFloat;

            Vector2 localPosOfClosestObstacle = Vector2.zero;

            foreach (Collider2D collider in colliders)
            {
                Vector2 localPos = _instance.transform.InverseTransformPoint(collider.gameObject.transform.position);

                if (localPos.y >= 0)
                {
                    if (collider.gameObject.GetComponent<CircleCollider2D>() == null)
                        continue;

                    float expandedRadius = collider.gameObject.GetComponent<CircleCollider2D>().radius + _instance.GetComponent<BoxCollider2D>().size.x / 2;

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
            steeringForce = _instance.transform.TransformVector(steeringForce);

            return steeringForce;
        }

        public Vector2 Separation(List<GameObject> neighbours)
        {
            Vector2 SteeringForce = Vector2.zero;

            foreach (GameObject gameObject in neighbours)
            {
                Vector2 ToAgent = _instance.transform.position - gameObject.transform.position;

                //scale the force inversely proportional to the agents distance  
                //from its neighbor.
                SteeringForce += ToAgent.normalized / ToAgent.magnitude;
            }

            return SteeringForce;
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

                AverageHeading -= _instance.GetComponent<Rigidbody2D>().velocity;
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
            return SteeringForce.normalized * _vehicle.MaxSpeed / 3;
        }

        public Vector2 Stop(Vector2 velocity, float timeToStop)
        {
            return velocity * -1 / timeToStop;
        }

        public Vector2 Pursuit(Vector2 evaderPos, Vector2 evaderVelocity)
        {
            //if the evader is ahead and facing the agent then we can just seek
            //for the evader's current position.
            Vector2 ToEvader = evaderPos - (Vector2)_instance.transform.position;

            double RelativeHeading = Vector2.Dot(_rigidbody2D.velocity.normalized, evaderVelocity.normalized);

            if ((Vector2.Dot(evaderVelocity.normalized, _rigidbody2D.velocity.normalized) > 0) && (RelativeHeading < -0.95))
            {
                return Seek(evaderPos);
            }

            //Not considered ahead so we predict where the evader will be.

            //the lookahead time is propotional to the distance between the evader
            //and the pursuer; and is inversely proportional to the sum of the
            //agent's velocities
            float LookAheadTime = evaderVelocity.magnitude / (_vehicle.MaxSpeed + evaderVelocity.magnitude);

            //now seek to the predicted future position of the evader
            return Seek(evaderPos + evaderVelocity * LookAheadTime);
        }

        public Vector2 Orbit(GameObject center, float radius)
        {
            //_instance.transform.RotateAround(center.transform.position, 20);
            Vector2 force = Vector2.zero;

            Vector2 local = _instance.transform.InverseTransformPoint(center.transform.position);

            Vector2 localVelocity = _instance.transform.InverseTransformVector(_rigidbody2D.velocity);

            force.x = localVelocity.y * local.normalized.x;

            force = _instance.transform.TransformVector(force);

            force = force.normalized * _vehicle.MaxSpeed * 5;

            return force;


            ////if the evader is ahead and facing the agent then we can just seek
            ////for the evader's current position.
            //Vector2 ToEvader = evader.transform.position - _instance.transform.position;

            //double RelativeHeading = Vector2.Dot(_rigidbody2D.velocity.normalized, evader.GetComponent<Rigidbody2D>().velocity.normalized);

            //if ((Vector2.Dot(evader.GetComponent<Rigidbody2D>().velocity.normalized, _rigidbody2D.velocity.normalized) > 0) && (RelativeHeading < -0.95))
            //{
            //    return Seek(evader.transform.position);
            //}

            ////Not considered ahead so we predict where the evader will be.

            ////the lookahead time is propotional to the distance between the evader
            ////and the pursuer; and is inversely proportional to the sum of the
            ////agent's velocities
            //float LookAheadTime = evader.GetComponent<Rigidbody2D>().velocity.magnitude / (_vehicle.MaxSpeed + evader.GetComponent<Rigidbody2D>().velocity.magnitude);

            ////now seek to the predicted future position of the evader
            //return Seek((Vector2)evader.transform.position + evader.GetComponent<Rigidbody2D>().velocity * LookAheadTime);
        }
    }
}
