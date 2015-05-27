using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Pathfinding;

[RequireComponent(typeof(Graph))]
public class GraphDrawer : MonoBehaviour
{
    private bool draw = false;
    
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKey(KeyCode.G))
	    {
	        draw = !draw;

            GameObject parent = GameObject.Find("Common/Graph");

	        if (draw)
	        {
                foreach (GraphNode node in Graph.Instance.GetNodes())
	            {
	                foreach (GraphEdge edge in node.Edges)
	                {
                        GraphNode otherNode = Graph.Instance.GetNode(edge.To);

                        Graph.Instance.DrawEdge(node, otherNode, Color.green);
	                }
	            }
	        }
	        else
	        {
	            foreach (Transform child in parent.transform)
	            {
	                Destroy(child.gameObject);
	            }
	        }
	    }
	}
}
