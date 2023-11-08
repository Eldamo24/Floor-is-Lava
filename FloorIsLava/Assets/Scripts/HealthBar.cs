using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //obtiene el slider del gameobject y dispone de metodos
    //para poder modificar el porcentaje del slider
    Slider _healthSlider;

    private void Start()
    {
        _healthSlider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        _healthSlider.value = maxHealth;
        _healthSlider.maxValue = maxHealth;
    }

    public void SetHealth(int health)
    {
        _healthSlider.value = health;
    }
}
