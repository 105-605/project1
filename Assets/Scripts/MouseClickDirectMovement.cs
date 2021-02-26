using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickDirectMovement : MonoBehaviour
{
    public float speed;
    Vector3 target;

    void Update()
    {
        
        if (Input.GetMouseButtonUp(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
        }
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
