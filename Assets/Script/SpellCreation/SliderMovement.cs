using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
//NORMALISER PAR RAPPORT A LA TAILLE
public class SliderMovement : MonoBehaviour
{
    private RectTransform rt;

    [SerializeField] private Camera camera;
    [SerializeField] private int inverted;
    private Slider s;

    private Vector3 beg, end;
    private PolygonCollider2D PC2;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        s = GetComponent<Slider>();
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        PC2 = GetComponent<PolygonCollider2D>();
        beg =s.handleRect.transform.position;
        s.value = 1;
        end = s.handleRect.transform.position;
        s.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            PC2.enabled = false;
            return;
        }

        Vector3 worldMousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.transform.position.z * -1));
        s.value = GetClosestPointOnLine(worldMousePosition);
        return;
        /*//Slider Up
        if (inverted == 0)
        {
            worldMousePosition /= 4f;
            //float y = Math.Clamp(Math.Abs(worldMousePosition.y), 0, 1);
            s.value = worldMousePosition.y;
        }
        //Slider Down LF
        else if (Math.Abs(inverted) == 2)
        {
            //worldMousePosition /= 3f;
            float x = worldMousePosition.x;
            s.value = (inverted / 2) * x;
        }
        //Slider UP LF
        else if (Math.Abs(inverted) == 1)
        {
            //worldMousePosition /= 2f;
            float x = worldMousePosition.x;
           
            s.value = inverted * x;
        }*/
    }
    private float GetClosestPointOnLine(Vector3 point)
    {
        Vector3 lineDirection = end - beg;
        float closestPoint = Vector3.Dot(point - beg, lineDirection) /
                             Vector3.Dot(lineDirection, lineDirection);
        return closestPoint = Mathf.Clamp01(closestPoint);
        
    }
}
