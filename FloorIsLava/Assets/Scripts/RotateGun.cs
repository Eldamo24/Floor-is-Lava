using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public Grappling grappling;

    private void Update()
    {
        if (!grappling.IsGrappling()) return;
        transform.LookAt(grappling.GetGrapplePoint());
    }
}
