using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun grapplingGun;

    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        desiredRotation = !grapplingGun.isGrappling() ? transform.parent.rotation : 
            Quaternion.LookRotation(grapplingGun.getGrapplePoint() - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime 
            * rotationSpeed);
    }
}
