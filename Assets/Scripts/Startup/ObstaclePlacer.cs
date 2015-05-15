using UnityEngine;
using System.Collections;

public class ObstaclePlacer : MonoBehaviour
{

    public bool Enabled = true;

    public int EstimatedObstacleCount;

	// Use this for initialization
	void Start ()
	{
	    if (!Enabled)
	        return;

        GameObject tree = Resources.Load<GameObject>("PreFabs/Tree");

	    int tiles = 39*2*19*2;

	    int i = tiles/EstimatedObstacleCount;

        for (int x = Settings.Instance.Width * -1 + 2; x <= Settings.Instance.Width - 2; x++)
        {
            for (int y = Settings.Instance.Height * -1 + 2; y <= Settings.Instance.Height - 2; y++)
            {
                if ((int)(Random.value * i) == 0)
                {
                    float scale = Random.value * 2.5f + 1.5f;

                    if (!Physics2D.OverlapCircle(new Vector2(x, y), scale * 2))
                    {
                        GameObject newRobot = Instantiate(tree, new Vector2(x, y), new Quaternion()) as GameObject;


                        newRobot.transform.localScale = new Vector3(scale, scale, 1);
                    }
                }
            }
        }
	}
}
