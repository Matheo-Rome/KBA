using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WanderHostile : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotSpeed = 300f;

    [SerializeField] private bool iswandering = false;
    [SerializeField] private bool isRotatingLeft = false;
    [SerializeField] private bool isRotatingRight = false;
    [SerializeField] private bool iswalking = false;

    [SerializeField] private EnemyController ec;

    private Rigidbody rb;
    void Start(){
        rb = GetComponent<Rigidbody>();
        // m_animator.SetInteger("AnimIndex", 1);
    }
 
    // Update is called once per frame
    void Update () {

        if (ec.following)
        {
            StopCoroutine("Wandering");
            iswandering = false;
            iswalking = false;
            isRotatingLeft = false;
            isRotatingRight = false;
        }

        if (iswandering == false && !ec.following);
        {
            StartCoroutine(Wandering());
        }
        if(isRotatingRight == true)
        {
            
            rb.velocity = Vector3.zero;
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        else if (isRotatingLeft == true)
        {
            rb.velocity = Vector3.zero;
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        else if(iswalking == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }


    IEnumerator Wandering()
    {
        if (!iswandering && !ec.following)
        {
            int rotTime = Random.Range(1, 2);
            int rotateWait = Random.Range(1, 2);
            int rotateLorR = Random.Range(0, 2);
            int walkWait = Random.Range(1, 2);
            int walkTime = Random.Range(4, 8);

            iswandering = true;

            yield return new WaitForSeconds(walkWait);
            iswalking = true;
            
            yield return new WaitForSeconds(walkTime);
            iswalking = false;

            yield return new WaitForSeconds(rotateWait);
            if (rotateLorR == 0)
            {
                isRotatingRight = true;
            }
            else 
            {
                isRotatingLeft = true;
            }

            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
            isRotatingRight = false;
            iswandering = false;
        }
    }
}