using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollowMouse : MonoBehaviour
{
    Camera viewCamera;
    float angle;
    Vector2 mousePos;
    void Start()
    {
        viewCamera = Camera.main;
    }

    void Update()
    {
        mousePos = viewCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward),1000*Time.deltaTime);    
    }
}
