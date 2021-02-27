using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed;

    void Update()
    {
        float x = cameraSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        float y = cameraSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
    }
}