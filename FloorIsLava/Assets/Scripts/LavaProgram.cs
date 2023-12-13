using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaProgram : MonoBehaviour
{
    [SerializeField]
    private GameObject lava;
    
    // Start is called before the first frame update
    //void Start()
    //{
    //    
    //}

    void Update()
    {
        lava.transform.Rotate(new Vector3(0f, 3.6f, 0f) * Time.deltaTime);
    }
}
