using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Medicine_Heal : MonoBehaviour
{
    public TMP_Text medicineText;

    public float medicines = 0;

    public PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        medicineText.text = "Medicines: "+medicines.ToString();

        if(Input.GetButtonDown("heal")&&medicines>=1)
        {
            medicines -= 1;
            heal();
        }
        if (Input.GetButtonDown("heal") && medicines >= 0)
        {
            return;
        }
    }

    public void heal()
    {
        playerHealth.heal(30);
    }
}
