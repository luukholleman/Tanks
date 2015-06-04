using UnityEngine;

namespace Assets.Scripts
{
    public class MainGUI : MonoBehaviour {

        void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width - 80, 20, 60, 20), "Exit"))
            {
                Application.Quit();
            }

            if (GUI.Button(new Rect(Screen.width - 80, 50, 60, 20), "Restart"))
            {
                Time.timeScale = 1f;
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    }
}
