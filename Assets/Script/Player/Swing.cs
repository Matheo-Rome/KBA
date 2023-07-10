using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Swing : MonoBehaviour
{

    [Header("input")] 
    [SerializeField] private KeyCode swingKey = KeyCode.Mouse0;

    [Header("References")] 
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform guntTip, cam, player;
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private FuelBar fb;
    [SerializeField] private GlobalAchievement gA;

    [Header("Swinging")]
    [SerializeField] private float maxSwingDsitance = 25f;
    private Vector3 swingPoint;
    public SpringJoint joint;

    [Header("Air")] 
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float horizontalThrustForce, fowardThrustForce, extendedCableSpeed;

    [Header("Prediction")] 
    [SerializeField] private RaycastHit predictionHit;
    [SerializeField] private float predictionSphereCastRadius;
    [SerializeField] private Transform predictionPoint;
    private Vector3 currentGrapplePosition;

    [Header("Values")]
    public int fuel = 8000;
    public int maxfuel = 8000;
    
    // Start is called before the first frame update
    void Start()
    {
        lr.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(swingKey))
            StartSwing();

        if(Input.GetKeyUp((swingKey)))
             StopSwing();
        
        CheckForSwingPoints();
        
        if (joint && fuel > 0)
            AirMovement();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartSwing()
    {
        if (predictionHit.point == Vector3.zero)
            return;

        gA.nb_grappling++;
        pm.swinging = true;

        //Create a Spring joint between the tip of the grapplin and the targeted point
        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        // Joint values
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;
        
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        //Give two slot to the line renderer
        lr.positionCount = 2;
        currentGrapplePosition = guntTip.position;
    

}

    private void StopSwing()
    {
        pm.swinging = false;
        // Remove the points from the line renderer
        lr.positionCount = 0;
        Destroy(joint);
    }

    private void DrawRope()
    {
        if (!joint)
            return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime );
        
        lr.SetPosition(0, guntTip.position);
        lr.SetPosition(1, swingPoint);
    }

    //Allow for nice movement while swinging at cost of "Fuel/Magic"
    private void AirMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        // go right
        if (Input.GetButton("Horizontal") && horizontalInput > 0)
        {
            fuel-=3;
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        }


        // go left
        if (Input.GetButton("Horizontal") && horizontalInput < 0)
        {
            fuel-=3;
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);
        }

        // go forward
        if (Input.GetButton("Vertical") && verticalInput > 0)
        {
            fuel -= 3;
            rb.AddForce((orientation.forward * fowardThrustForce * Time.deltaTime));
        }



        // retract grapple
        if (Input.GetKey(KeyCode.Space))
        {
            fuel-=5;
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * fowardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        
        //extract grapple
        if (Input.GetButton("Vertical") && verticalInput < 0)
        {
            fuel-=6;
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendedCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
        
        fb.DecrementFuel(fuel);
    }

    public void IncrementFuel()
    {
        fb.IncrementFuel(fuel);
    }
    
    private void CheckForSwingPoints()
    {
        if (joint != null)
            return;

        RaycastHit sphereCastHit;
        RaycastHit raycastHit; 
        
        Vector3 realHitPoint = Vector3.zero;
        
        //Test if we have a grappleable collider in line of sight 
        Physics.Raycast(cam.position, cam.forward, out raycastHit, maxSwingDsitance, whatIsGrappleable);

        //Keep the hit point if there is one
        if (raycastHit.point != Vector3.zero)
        {
            realHitPoint = raycastHit.point;
            predictionHit = raycastHit;
        }
        else
        {
            //SphereCast to get the closest point from our camera to be the hit point
            Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxSwingDsitance,
                whatIsGrappleable);
            if (sphereCastHit.point != Vector3.zero)
            {
                realHitPoint = sphereCastHit.point;
                predictionHit = sphereCastHit;
            }
        }

        //if we have a hit point activate the point
        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else
            predictionPoint.gameObject.SetActive(false);

    }
}
