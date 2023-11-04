using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{

    public ProgressBar Pb;
    public int Health = 100;
    void Update()
    {
        Pb.BarValue = Health;
    }
}
