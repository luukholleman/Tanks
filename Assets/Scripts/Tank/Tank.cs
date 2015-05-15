using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.StateMachines;
using Assets.Scripts.StateMachines.Messaging;

[RequireComponent(typeof(Vehicle))]
public class Tank : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GetComponent<Vehicle>().Side == Player.Side.Blue)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/tankBlue_outline");

            foreach (Transform t in transform)
            {
                t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/barrelBlue_outline");
            }
        }
        else if (GetComponent<Vehicle>().Side == Player.Side.Red)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/tankRed_outline");

            foreach (Transform t in transform)
            {
                t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/barrelRed_outline");
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
