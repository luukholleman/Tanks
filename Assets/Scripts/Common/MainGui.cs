using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts.Pathfinding;
using Assets.Scripts.StateMachines;

public class MainGui : MonoBehaviour {

	// Use this for initialization
	void OnGUI ()
    {
        if (GUI.Button(new Rect(0, 0, 100, 20), "New tank"))
        {
            GameObject tank = Resources.Load<GameObject>("PreFabs/tank");
            GameObject newTank = Instantiate(tank, new Vector3(-35, 0, 0), new Quaternion()) as GameObject;

            //AStar AStar = new AStar(Graph.Instance, Graph.Instance.GetNode((Vector2)newTank.transform.position).Index, (int)(Random.value * 2000));
            AStar aStar = new AStar(Graph.Instance, Graph.Instance.GetNode((Vector2)newTank.transform.position).Index, (int)(Random.value * Graph.Instance.NodeCount));

            //AStar.Path.Reverse();

            newTank.GetComponent<StateMachine>().CurrentState = new FollowPath(aStar.Path);
        }

        if (GUI.Button(new Rect(0, 20, 100, 20), "New Robot Flock"))
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    GameObject robot = Resources.Load<GameObject>("PreFabs/Robot");
                    GameObject newRobot = Instantiate(robot, new Vector3(35 + x, 0 + y, 0), new Quaternion()) as GameObject;

                    newRobot.GetComponent<StateMachine>().CurrentState = new GoToPointState(new Vector2(20 + x, 0 + y));
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
