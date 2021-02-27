using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textinfo : MonoBehaviour
{
    public GameObject lastHit;
    bool isActive = false;
    private void Start()
    {
        lastHit = gameObject;
    }


    private void Update()

    {
        Vector3 mousePos = Input.mousePosition;
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        

        RaycastHit2D ray = Physics2D.Raycast(mousePos2d, Vector2.zero);

        if (ray.collider !=null)
        {
            GameObject hit = ray.collider.gameObject;
            
         
                lastHit.transform.Find("InfoText").gameObject.SetActive(false);
                lastHit = hit;
                if (hit.CompareTag("UI"))
                {
                    isActive = true;
                    hit.transform.Find("InfoText").gameObject.SetActive(true);
                }
       
        }
        else if (isActive)
        {
            isActive = false;
            lastHit.transform.Find("InfoText").gameObject.SetActive(false);
        }
    }
}
