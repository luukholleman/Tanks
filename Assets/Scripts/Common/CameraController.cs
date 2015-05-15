using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private Vector3 _startPosition;
    private Vector3 _origin;
    private Vector3 _diference;
    private bool _drag;
    private float _size;

    void Start()
    {
        _startPosition = Camera.main.transform.position;
        _size = Camera.main.GetComponent<Camera>().orthographicSize;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            _diference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;

            if (_drag == false)
            {
                _drag = true;
                _origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            _drag = false;
        }

        if (_drag == true)
        {
            Camera.main.transform.position = _origin - _diference;
        }

        float delta = Input.GetAxis("Mouse ScrollWheel");

        if (delta > 0)
        {
            Camera.main.GetComponent<Camera>().orthographicSize -= 3;
        }
        else if (delta < 0)
        {
            Camera.main.GetComponent<Camera>().orthographicSize += 3;
        }


        if (Input.GetKeyUp(KeyCode.Space)) // reset camera on space
        {
            Camera.main.transform.position = _startPosition;
            Camera.main.GetComponent<Camera>().orthographicSize = _size;
        }
    }
}
