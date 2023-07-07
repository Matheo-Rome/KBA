using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Swing : MonoBehaviour
{

    [Header("input")] [SerializeField] private KeyCode swingKey = KeyCode.Mouse0;

    [Header("References")] 
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform guntTip, cam, player;
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private PlayerMovement pm;

    [Header("Swinging")]
    [SerializeField] private float maxSwingDsitance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("Air")] 
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float horizontalThrustForce, fowardThrustForce, extendedCableSpeed;

    [Header("Prediction")] 
    [SerializeField] private RaycastHit predictionHit;
    [SerializeField] private float predictionSphereCastRadius;
    [SerializeField] private Transform predictionPoint;
    
    private Vector3 currentGrapplePosition;

    [SerializeField] private GlobalAchievement gA;
    
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
        
        if (joint)
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

        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = guntTip.position;
    

}

    private void StopSwing()
    {
        pm.swinging = false;
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

    private void AirMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        // go right
        if (Input.GetButton("Horizontal") && horizontalInput > 0)
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        

        // go left
        if (Input.GetButton("Horizontal") && horizontalInput < 0)
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);

        // go forward
        if (Input.GetButton("Vertical") && verticalInput > 0)
            rb.AddForce((orientation.forward * horizontalThrustForce * Time.deltaTime));
        


        // retract grapple
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * fowardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        
        //extract grapple
        if (Input.GetButton("Vertical") && verticalInput < 0)
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendedCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }

    private void CheckForSwingPoints()
    {
        if (joint != null)
            return;

        RaycastHit sphereCastHit;
        RaycastHit raycastHit; 
        
        Vector3 realHitPoint = Vector3.zero;
        
        Physics.Raycast(cam.position, cam.forward, out raycastHit, maxSwingDsitance, whatIsGrappleable);

        if (raycastHit.point != Vector3.zero)
        {
            realHitPoint = raycastHit.point;
            predictionHit = raycastHit;
        }
        else
        {
            Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxSwingDsitance,
                whatIsGrappleable);
            if (sphereCastHit.point != Vector3.zero)
            {
                realHitPoint = sphereCastHit.point;
                predictionHit = sphereCastHit;
            }
        }

        if (realHitPoint != Vector3.zero)
        {
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

    }
}
