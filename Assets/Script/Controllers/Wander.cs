using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wander : MonoBehaviour
{
	public float duration;    //the max time of a walking session (set to ten)
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

    

    private string[] m_buttonNames = new string[] { "Idle", "Run", "Dead" };

    private Animator m_animator;

    void Start(){
        randomX =  Random.Range(-3,3);
        randomZ = Random.Range(-3,3);
        m_animator = GetComponent<Animator>();
       // m_animator.SetInteger("AnimIndex", 1);
    }

    void Update ()
    {
        if (moveDirection == Vector3.zero)
            m_animator.SetFloat("Speed", 0);
        else
            m_animator.SetFloat("Speed", 1);  
        
            
        if (elapsedTime < duration && move) 
        {
            //if its moving and didn't move too much
            moveDirection = new Vector3(randomX,0,randomZ);
            actualPos = moveDirection;
            transform.Translate (moveDirection * Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp (a: transform.rotation, b: Quaternion.LookRotation (moveDirection), t: 1);
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
        }

        oldPos = moveDirection;
    }
}