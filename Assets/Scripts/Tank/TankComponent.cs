using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.StateMachines;
using Assets.Scripts.StateMachines.Messaging;

namespace Assets.Scripts.Tank
{
public class TankComponent : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	    name = new Namer().Name;

        if (GetComponent<Vehicle>().Side == Player.Side.Blue)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/tankBlue_outline");

            foreach (Transform t in transform)
            {
                if(t.name == "Barrel")
                    t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/barrelBlue_outline");
            }
        }
        else if (GetComponent<Vehicle>().Side == Player.Side.Red)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/tankRed_outline");

            foreach (Transform t in transform)
            {
                if (t.name == "Barrel")
                    t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tanks/barrelRed_outline");
            }
        }
	}
}
}
