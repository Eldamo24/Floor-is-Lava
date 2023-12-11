using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarBehaviour : MonoBehaviour
{
    private Text _healthTextComponent;
    private Slider _barFillSliderComponent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int _currentHealth = GameManager.gameManager._playerHealth.Health;
        _healthTextComponent.text = $"Vida {_currentHealth.ToString()}%";
        _barFillSliderComponent.value = _currentHealth;

    }

    private void Awake()
    {
        _healthTextComponent = transform.Find("Text").GetComponent<Text>();
        _barFillSliderComponent = transform.Find("BarFill").GetComponent<Slider>();
    }
}
