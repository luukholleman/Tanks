using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{

    public float ReloadSpeed;

    public float MovementSpeed;

    public float Repair;

    public bool NewTank;


	// Use this for initialization
	void Start () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            collision.gameObject.GetComponent<Vehicle>().MaxSpeed += MovementSpeed;
            collision.gameObject.GetComponent<Vehicle>().ReloadSpeed += ReloadSpeed;
            collision.gameObject.GetComponent<Vehicle>().Health += Repair;
        }

        Destroy(gameObject);
    }
}
