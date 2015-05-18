using UnityEngine;
using System.Collections;
using Assets.Scripts.StateMachines;

public class Commander : MonoBehaviour
{

    private GameObject _clicked;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Tank"))
            {
                _clicked = hit.collider.gameObject;
            }
        }

        if (Input.GetButtonUp("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_clicked != null)
            {
                //_clicked.GetComponent<StateMachine>().CurrentState = new GoToPointState(new Vector2(ray.origin.x, ray.origin.y));
            }
        }
	}
}
