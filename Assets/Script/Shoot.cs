using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private TMP_Text text;

    private int object_destroyed = 0;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "Object Destroyed : 0";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
           
            RaycastHit hit;
            bool res = Physics.Raycast(ray, out hit);
            if (res)
            {
                object_destroyed++;
                Destroy(hit.collider.gameObject);
                text.text = "Object Destroyed : " + object_destroyed;
            }
        }
    }
}
