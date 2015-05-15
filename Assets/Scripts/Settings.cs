using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{
    // Use this for initialization
    public static bool DebugState = false;

    public static Settings Instance;

    public int Width;
    public int Height;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DebugState = !DebugState;
        }
    }
}
