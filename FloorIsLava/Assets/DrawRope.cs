using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRope : MonoBehaviour
{   
    private Transform player;
    private LineRenderer lineRenderer;
    private Vector3 ropeEnd;

    void Start()
    {
        // Player/DescendingFeature/Stake/Rope (Player is grand-grand-father of Rope)
        player = transform.parent.transform.parent.transform.parent;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.grey;
        lineRenderer.endColor = Color.grey;
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        // I know nothing about materials, I copy-pasted this line to avoid purple line
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        lineRenderer.SetPosition(0, gameObject.transform.position);
        ropeEnd.x = gameObject.transform.position.x;
        ropeEnd.y = player.transform.position.y-0.2f;
        ropeEnd.z = gameObject.transform.position.z;
               
        lineRenderer.SetPosition(1, ropeEnd);          
    }
}
