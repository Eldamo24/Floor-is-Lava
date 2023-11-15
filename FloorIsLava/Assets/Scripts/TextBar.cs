using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TextBar : MonoBehaviour
{
    [SerializeField]
    public HealthBar healthBar;
    Text text;
    Slider sliderBar;
    Image image;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        healthBar = FindObjectOfType<HealthBar>();
        sliderBar = FindObjectOfType<Slider>();
        image = GameObject.Find("Canvas/UI-Gameplay/UI ProgressBar/BarFill/FillColor").GetComponent<Image>();
        audioSource = GameObject.Find("Canvas/UI-Gameplay/UI ProgressBar/BarFill/FillColor").GetComponent<AudioSource>();



    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            text.text = "Salud " + sliderBar.value + " %";
            if (sliderBar.value <= 30)
            {
                image.color = Color.red;
                audioSource.Play(); // Hay que agregar el audio en el audiosource del FillColor
            }
        }
        catch
        {
            Console.Error.WriteLine();
        }

    }
}
