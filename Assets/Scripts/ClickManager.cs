using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public List<GameObject> selected;
    bool controlling = false;
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D ray = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (ray.collider != null)
            {
                GameObject hit = ray.collider.gameObject;
                if (hit.CompareTag("controllable"))
                {
                    if (!controlling)
                        flush();
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        selected.Add(hit);
                        controlling = true;
                        hit.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else
                    {
                        flush();
                        controlling = true;
                        selected.Add(hit);
                        hit.GetComponent<SpriteRenderer>().color = Color.red;

                    }
                }
                if (hit.CompareTag("friendlyBuilding"))
                {
                    flush();
                    controlling = false;
                    selected.Add(hit);
                    hit.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else flush();
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (controlling)
            {
                foreach(GameObject g in selected)
                {
                    g.GetComponent<Unit>().GoToClick(Camera.main.ScreenToWorldPoint( Input.mousePosition));
                }
            }
        }
    }

    void flush()
    {
        foreach (GameObject g in selected)
        {
            if (g.GetComponent<SpriteRenderer>())
                g.GetComponent<SpriteRenderer>().color = Color.white;
        }
        selected.Clear();
    }
}
