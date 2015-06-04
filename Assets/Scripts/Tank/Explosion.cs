using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Explosion : MonoBehaviour
{
    public Player.Side Side;

	void Start ()
	{
	    int explosion = (int) (Random.value*6);

	    string color;

	    if (Side == Player.Side.Blue)
	        color = "Blue";
	    else
	        color = "Orange";

        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Smoke/smoke" + color + explosion);

        Destroy(gameObject, 10f); // lifetime of 10 seconds
	}
	
	void Update ()
    {
        // fade the smoke out
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, GetComponent<SpriteRenderer>().color.a - 0.5f * Time.deltaTime);
	}
}
