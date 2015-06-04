using Assets.Scripts;
using Assets.Scripts.Tank;
using UnityEngine;

public class Flag : MonoBehaviour
{

    public float Score;

    public readonly float MaxScore = 10;

    public Player.Side Side = Player.Side.None;

    public float CappingRange = 2;

    public float ScorePerTank = 0.3f;

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

            if (tank.GetComponent<Tank>().Side == Player.Side.Blue)
            {
                Score += ScorePerTank * Time.deltaTime;
            }
            else if (tank.GetComponent<Tank>().Side == Player.Side.Red)
            {
                Score -= ScorePerTank * Time.deltaTime;
            }
        }

        if (Score > MaxScore)
            Score = MaxScore;
        else if (Score < -MaxScore)
            Score = -MaxScore;

        if(Score > 8)
            Side = Player.Side.Blue;
        else if (Score < -8)
            Side = Player.Side.Red;
        else
            Side = Player.Side.None;
    }
}
