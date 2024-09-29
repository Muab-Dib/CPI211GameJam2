using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFilled : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100;
    public float currenthealth;
    [SerializeField] private TextMeshProUGUI healthtext;
    private float lerpSpeed = 0.005f;

    void Start()
    {
        currenthealth=maxHealth;
        healthtext.text= "Health: " + currenthealth;
    }

    private void Update()
    {
        if(healthSlider.value > currenthealth)
        {
            healthSlider.value = currenthealth;
            
        }
        else if (healthSlider.value < currenthealth)
        {
            easeHealthSlider.value = currenthealth;
            
        }
        if (currenthealth < easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currenthealth, lerpSpeed);
        }
        else if (healthSlider.value < currenthealth)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, currenthealth, lerpSpeed);
        }

        healthtext.text = "Health: " + currenthealth;
    }

    public void UpdateHealth(float newHealth)
    {
        currenthealth = newHealth; 
    }

}
