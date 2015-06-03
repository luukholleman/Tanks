using UnityEngine;

namespace Assets.Scripts
{
    public class Spawn : MonoBehaviour
    {
        public Player.Side Side;

        public int TankCount;

        // Use this for initialization
        void Start()
        {
            GameObject tank = Resources.Load<GameObject>("PreFabs/tank");
            for (int i = 0; i < TankCount; i++)
            {
                GameObject newTank = Instantiate(tank, transform.position + new Vector3(0, -TankCount / 2 + i), new Quaternion()) as GameObject;

                newTank.transform.parent = GameObject.Find("Tanks").transform;

                newTank.GetComponent<Tank.Tank>().Side = Side;

            }
        }
    }
}
