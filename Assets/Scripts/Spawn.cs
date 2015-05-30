using Assets.Scripts.StateMachines;
using Assets.Scripts.StateMachines.Base;
using UnityEngine;

namespace Assets.Scripts
{
    public class Spawn : MonoBehaviour
    {
        public Player.Side Side;

        public int TankCount;

        // Use this for initialization
        void Start()
        {
            GameObject tank = Resources.Load<GameObject>("PreFabs/tank");
            for (int i = 0; i < TankCount; i++)
            {
                GameObject newTank = Instantiate(tank, transform.position + new Vector3(0, -TankCount / 2 + i), new Quaternion()) as GameObject;

                newTank.transform.parent = GameObject.Find("Tanks").transform;

                ////AStar dijkstra = new AStar(Graph.Instance, Graph.Instance.GetNode((Vector2)newTank.transform.position).Index, (int)(Random.value * 2000));
                //AStar dijkstra = new AStar(Graph.Instance, Graph.Instance.GetNode(newTank.transform.position).Index, (int)(Random.value * Graph.Instance.NodeCount));

                ////dijkstra.Path.Reverse();

                //newTank.GetComponent<StateMachine>().CurrentState = new FollowPath(dijkstra.Path);
                //newTank.GetComponent<StateMachine>().CurrentState = new PatrolState();

                newTank.GetComponent<Vehicle>().Side = Side;

            }
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}
