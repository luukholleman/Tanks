using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Common
{
    class TreeSizer : MonoBehaviour
    {
        void Awake()
        {
            float scale = Random.value*1.5f + 4.5f;
            transform.localScale = new Vector3(scale, scale, scale);
            transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
        }
    }
}
