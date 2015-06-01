using System;
using UnityEngine;

namespace Assets.Scripts.Tank
{
    public class ChatBubble : MonoBehaviour
    {
        public String Text;

        public GUIStyle Style;

        void Start()
        {

        }
        
        void OnGUI()
        {
            if (Settings.DebugState)
            {
                var point = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2, 0));

                GUI.Label(new Rect(point.x - 100, Screen.currentResolution.height - point.y, 200, 200), transform.parent.name + "\n" + Text, Style);
            }
        }
    }
}
