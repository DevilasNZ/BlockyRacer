using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple script to animate the rotation of the car display on the car selector.
public class CarSelectorRotation : MonoBehaviour
{
    public float speed = 1f;
    public float playerRotSpeed = 20f;

    bool rotating = true;
    
    // Update is called once per frame
    void Update()
    {
        if(rotating)
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }

    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * playerRotSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotX);
    }

    void OnMouseDown()
    {
        rotating = false;
    }

    void OnMouseUp()
    {
        rotating = true;
    }
}
