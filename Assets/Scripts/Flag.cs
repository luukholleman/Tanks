using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Flag : MonoBehaviour
{

    public float Score = 0;

    public Player.Side Side = Player.Side.None;

    public float CappingRange = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    UpdateSide();

	    switch (Side)
        {
            case Player.Side.None:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Flags/white");
                break;
            case Player.Side.Red:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Flags/red");
                break;
            case Player.Side.Blue:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Flags/blue");
                break;
	    }
	}

    void UpdateSide()
    {
        foreach (Transform tank in GameObject.Find("Tanks").transform)
        {
            if(Vector2.Distance(tank.position, transform.position) > CappingRange)
                continue;

            if (tank.GetComponent<Vehicle>().Side == Player.Side.Blue)
            {
                Score += 1 * Time.fixedDeltaTime;
            }
            else if (tank.GetComponent<Vehicle>().Side == Player.Side.Red)
            {
                Score -= 1 * Time.fixedDeltaTime;
            }
        }

        if (Score > 10)
            Score = 10;
        else if (Score < -10)
            Score = -10;

        if(Score > 8)
            Side = Player.Side.Blue;
        else if (Score < -8)
            Side = Player.Side.Red;
        else
            Side = Player.Side.None;
    }
}
