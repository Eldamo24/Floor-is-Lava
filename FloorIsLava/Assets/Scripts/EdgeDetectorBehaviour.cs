using System.Collections;
using System.Collections.Generic; // to use a List.
using UnityEngine;

public class EdgeDetectorBehaviour : MonoBehaviour
{
    private GameObject[] referencePoint = new GameObject[4];
    void Start()
    {
        // For sure must there be a more compact and efficient way to write this
        referencePoint[0] = GameObject.Find("/Player/PlayerObj/EdgeDetector/Point0");
        referencePoint[1] = GameObject.Find("/Player/PlayerObj/EdgeDetector/Point1");
        referencePoint[2] = GameObject.Find("/Player/PlayerObj/EdgeDetector/Point2");
        referencePoint[3] = GameObject.Find("/Player/PlayerObj/EdgeDetector/Point3");
    }

    // It tells if there is not any contact with nothing just in front
    public bool detectEdge()
    {
        for (int i = 0; i < 4; i++)
            if (Physics.Raycast(referencePoint[i].transform.position, Vector3.down, 2) || Physics.Raycast(referencePoint[i].transform.position, Vector3.up, 1.85f))
                return false;
        return true;
    }
    
}
