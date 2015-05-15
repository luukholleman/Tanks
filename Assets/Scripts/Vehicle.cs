using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.StateMachines;
using Assets.Scripts.StateMachines.Messaging;
using Assets.Scripts.Tank;

[RequireComponent(typeof(BoxCollider2D))]
public class Vehicle : MonoBehaviour
{
    public float MaxSpeed;

    public float ReloadSpeed;

    public float Health = 100;

    public Player.Side Side;

    public static int num = 1;

    private Vector3[] _directions = new Vector3[num];

    private int _key;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (Health <= 0)
        {
            GameObject dropper = GameObject.Find("PowerUpDropper");

            Message msg = new Message(gameObject, dropper, Message.MessageType.TankDied);

            Messenger.BroadcastMessage(msg);

            Messenger.Dispatch();

            gameObject.SetActive(false);

            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        Vector3 moveDirection = (Vector3)GetComponent<Rigidbody2D>().velocity;

        _directions[_key++] = moveDirection;

        if (_key == num)
            _key = 0;

        float angle = 0;

        foreach (Vector3 direction in _directions)
        {
            angle += Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        angle /= num;

        angle -= 90;

        angle = angle % 360;

        Vector3 rotNew = new Vector3(0, 0, angle);

        transform.rotation = Quaternion.Euler(rotNew);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rocket") && collision.gameObject.GetComponent<Rocket>().Side != Side)
        {
            Health -= collision.gameObject.GetComponent<Rocket>().Damage;
        }
    }
}
