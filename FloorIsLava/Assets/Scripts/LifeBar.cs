using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    public GameObject Player;
    public ProgressBar Pb;
    public int Health = 100;
    void Update()
    {
        Pb.BarValue = Health;
        if (Health <=0 )
        {
            Destroy(Player);
            Debug.Log("Cargar escena o menu de GAME OVER"); //reemplazar linea con lo que corresponda
        }



    }
}
