using UnityEngine;
using System.Collections;

public class WallPlacer : MonoBehaviour {

    public bool Enabled = true;

    // Use this for initialization
    void Start()
    {
        if (!Enabled)
            return;

        GameObject wall = Resources.Load<GameObject>("PreFabs/TreeWall");

        for (int x = Settings.Instance.Width * -1 - 1; x <= Settings.Instance.Width + 1; x++)
        {
            for (int y = Settings.Instance.Height * -1 - 1; y <= Settings.Instance.Height + 1; y++)
            {
                if (x == Settings.Instance.Width * -1 - 1 || y == Settings.Instance.Height * -1 - 1 || x == Settings.Instance.Width + 1 || y == Settings.Instance.Height + 1)
                    Instantiate(wall, new Vector3(x, y, 0), new Quaternion());
            }
        }
	}
	
}
