using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine_PickUp : MonoBehaviour
{
    private bool inReach;
    public GameObject pickUpText;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        inReach = false;
        pickUpText.SetActive(false);
        //player = GameObject.Find("FirstPersonController");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Reach")
        {
            inReach = true;
            pickUpText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            pickUpText.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact")&&inReach)
        {
            player.GetComponent<Medicine_Boost>().medicines += 1;
            inReach=false;
            pickUpText.SetActive(false);
            Destroy(gameObject);
        }
    }
}
