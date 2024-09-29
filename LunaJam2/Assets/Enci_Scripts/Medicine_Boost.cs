using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Medicine_Boost : MonoBehaviour
{
    public TMP_Text medicineText;


    public float multiplier = 2f;
    public float powerUpDuration = 10f;
    public float medicines = 0;

    //public PlayerHealth playerHealth;
    public FirstPersonController fpsModular;
    //public FirstPersonMovement fpsMini;
    public StatusEffectManager effectManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        medicineText.text = "Medicines: "+medicines.ToString();


        if(Input.GetButtonDown("boost")&&medicines>=1&&fpsModular.powerupEnabled==false)
        {
            medicines -= 1;
            PowerUp();
        }
        if ((Input.GetButtonDown("boost") && medicines >= 0)||fpsModular.powerupEnabled==true)
        {
            return;
        }


    }



    public void PowerUp()
    {
        effectManager.StartBoostEffect(powerUpDuration);
        fpsModular.powerupEnabled=true;
        fpsModular.walkSpeed=15f;
        fpsModular.sprintSpeed=20f;
        
        
        fpsModular.walkSpeed=5f;
        fpsModular.sprintSpeed=7f;
        fpsModular.powerupEnabled=false;
    }
    
}
