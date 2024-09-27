using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth=100;
    public HealthBarFilled healthBarFilled;
    [SerializeField] private TextMeshProUGUI healthtext;

    // Start is called before the first frame update
    void Start()
    {
        health=maxHealth;
        healthBarFilled.currenthealth=maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateHealth()
    {
        health=Mathf.Clamp(health,0,100);
        healthtext.text= "Health: " + health;

        healthBarFilled.UpdateHealth(health);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateHealth();
        if(health<=0)
        {
            Debug.Log("died!");
        }
    }

    public void heal(int amount)
    {
        health += amount;
        UpdateHealth();
    }
}
