using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    //DEPRECEATED BUT KEPT BECAUSE CAN BE USEFUL
    [Header("References")]
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform gunTip;
    [SerializeField] private LayerMask Grappleable;
    [SerializeField] private LineRenderer lr;

    [Header("Grappling")]
    [SerializeField] private float GrappleRange;
    [SerializeField] private float grappleDelay;
    [SerializeField] private float overShootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    [SerializeField] private float grapplingCd;
    [SerializeField] private float grapplingCdTimer;

    [Header(("Input"))] [SerializeField] private KeyCode grappleKey = KeyCode.Mouse1;
    
    public bool IsGrappling;
    void Start()
    {
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(grappleKey))
            StartGrapple();
        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (IsGrappling)
            lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {
        //pm.freeze = true;
        if (grapplingCdTimer > 0)
            return;
        IsGrappling = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, GrappleRange, Grappleable))
        {
            grapplePoint = hit.point;
            
            Invoke(nameof(ExecuteGrapple), grappleDelay);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * GrappleRange;
            
            Invoke(nameof(StopGrapple), grappleDelay);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overShootYAxis;

        if (grapplePointRelativeYPos < 0)
            highestPointOnArc = overShootYAxis;
        
        //pm.JumpToPosition(grapplePoint,highestPointOnArc);
        
        Invoke(nameof(StopGrapple), 1f); // faire coroutine
    }

    public void StopGrapple()
    {
        if (IsGrappling)
        {
            pm.freeze = false;
            IsGrappling = false;
            grapplingCdTimer = grapplingCd;
            lr.enabled = false;
        }
    }
}
