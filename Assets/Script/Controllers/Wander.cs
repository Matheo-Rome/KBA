using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Wander : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotSpeed = 300f;

    private bool iswandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool iswalking = false;

    private string[] m_buttonNames = new string[] { "Idle", "Run", "Dead" };

    private Animator m_animator;

    private Rigidbody rb;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        iswandering = false;
        // m_animator.SetInteger("AnimIndex", 1);
    }
 
    // Update is called once per frame
    void Update () {
        // apply the desired movement or start the coroutine if not started
        if (!iswandering)
        {
            StartCoroutine(Wandering());
        }
        if(isRotatingRight)
        {
            rb.velocity = Vector3.zero;
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        else if (isRotatingLeft)
        {
            rb.velocity = Vector3.zero;
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        else if(iswalking)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }


    //Make the entity walk, wait, turn,wait with random timing
    IEnumerator Wandering()
    {
        if (!iswandering)
        {
            int rotTime = Random.Range(1, 2);
            int rotateWait = Random.Range(1, 3);
            int rotateLorR = Random.Range(0, 2);
            int walkWait = Random.Range(1, 3);
            int walkTime = Random.Range(3, 7);

            iswandering = true;

            yield return new WaitForSeconds(walkWait);
            m_animator.SetFloat("Speed", 1);
            iswalking = true;
            
            yield return new WaitForSeconds(walkTime);
            iswalking = false; 
            m_animator.SetFloat("Speed", 0);
            
            yield return new WaitForSeconds(rotateWait);
            if (rotateLorR == 0)
            { 
                m_animator.SetFloat("Speed", 1);
                isRotatingRight = true;
            }
            else 
            {
                m_animator.SetFloat("Speed", 1);
                isRotatingLeft = true;
            }

            yield return new WaitForSeconds(rotTime);
            m_animator.SetFloat("Speed", 0);
            isRotatingLeft = false;
            isRotatingRight = false;
            iswandering = false;
        }
       
    }
	/*public float duration;    //the max time of a walking session (set to ten)
    float elapsedTime   = 0f; //time since started walk
    float wait          = 0f; //wait this much time
    float waitTime      = 0f; //waited this much time
    public float speed = 0.5f;

    float randomX;  //randomly go this X direction
    float randomZ;  //randomly go this Z direction

    bool move = true; //start moving

    Vector3 moveDirection;

    Vector3 actualPos = Vector3.zero;
    Vector3 oldPos = Vector3.zero;
    [SerializeField] private CharacterController ch;
    [SerializeField] private Rigidbody rb;

    

    private string[] m_buttonNames = new string[] { "Idle", "Run", "Dead" };

    private Animator m_animator;

    void Start(){
        randomX =  Random.Range(-3,3);
        randomZ = Random.Range(-3,3);
        m_animator = GetComponent<Animator>();
       // m_animator.SetInteger("AnimIndex", 1);
    }

    private Vector3 pos;
    private bool changeDir = false;
    void Update ()
    {
        // Animation
        if (moveDirection == Vector3.zero)
            m_animator.SetFloat("Speed", 0);
        else
            m_animator.SetFloat("Speed", 1);  
        
        
        // Movement
        if (elapsedTime < duration && move) 
        {
            //if its moving and didn't move too much
            moveDirection = new Vector3(randomX,0,randomZ);
            actualPos = moveDirection;
            transform.Translate(moveDirection * Time.deltaTime * speed);
            Vector3 direction = rb.velocity.normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction), Time.deltaTime * 2f );
            elapsedTime += Time.deltaTime;
        } 
        else if (move)
        {
            //do not move and start waiting for random time
            move        = false;
            wait        = Random.Range (5, 10);
            waitTime    = 0f;
            moveDirection = Vector3.zero;
        }

        if (waitTime < wait && !move) 
        {
            //you are waiting
            
            moveDirection = Vector3.zero;
            waitTime += Time.deltaTime;
        } 
        else if(!move)
        {
            move = true;
            
            moveDirection = Vector3.zero;
            elapsedTime = 0f;
            randomX = Random.Range(-3,3);
            randomZ = Random.Range(-3,3);
            pos = transform.position;
            changeDir = true;
        }

        oldPos = moveDirection;
    }*/
}