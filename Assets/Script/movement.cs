using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float zMove = Input.GetAxis("Horizontal"); // d key changes value to 1, a key changes value to -1
        float xMove = -1 * Input.GetAxis("Vertical"); // w key changes value to 1, s key changes value to -1
        rb.velocity = new Vector3(xMove, rb.velocity.y, zMove) * speed;
    }
}
