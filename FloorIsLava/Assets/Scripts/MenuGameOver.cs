using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour
{
    public Canvas barra;
    public Canvas menuGO;
    public bool statusBar = true;
    public bool statusGO = false;

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuInicial(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void Salir()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


    // Start is called before the first frame update
    void Start()
    {
        barra = GetComponent<Canvas>();
        menuGO = GetComponent<Canvas>();

    }

    // Update is called once per frame
    void Update()
    {
        if (statusGO == true)
        {
            statusBar = false;
        }
    }
}
