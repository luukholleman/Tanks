using UnityEngine;
using System.Collections;
using Assets.Scripts.StateMachines;
using Assets.Scripts.StateMachines.PowerUp;

public class PowerUpDropper : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    GetComponent<StateMachine>().CurrentState = new IdleState();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
