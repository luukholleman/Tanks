using UnityEngine;

public class Settings : MonoBehaviour
{
    public static bool DebugState;

    public static Settings Instance;

    public int Width = 60;
    public int Height = 36;

    public int TankSpawnDelay = 20;

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
