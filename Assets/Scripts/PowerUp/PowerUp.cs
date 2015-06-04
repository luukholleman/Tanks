using System;
using Assets.Scripts.Tank;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float ReloadSpeed;

    public float MovementSpeed;

    public float Repair;

    public bool NewTank;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            collision.gameObject.GetComponent<Tank>().MaxSpeed += MovementSpeed;
            collision.gameObject.GetComponent<Tank>().ReloadSpeed += ReloadSpeed;
            collision.gameObject.GetComponent<Tank>().Health += Repair;

            collision.gameObject.GetComponent<Tank>().Health = Math.Min(collision.gameObject.GetComponent<Tank>().Health, 100);
        }

        Destroy(gameObject);
    }
}
