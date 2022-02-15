using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class AmmoPickup : MonoBehaviour
{
    private GameObject pickUp;
    private GameObject pressE;
    public string ammoTag;
    public int ammoPerBox;
    public PlayerShooting weaponScript;

    void Start()
    {
   
    }

    void Update()
    {
        if (pressE != null && Input.GetKeyDown(KeyCode.E))
        {
            PickUpAmmo(); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            pickUp = other.gameObject;
            pressE = other.transform.GetChild(0).GetChild(0).gameObject;
            pressE.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            if (pressE != null)
                pressE.SetActive(false);
            pickUp = null;
            pressE = null;
        }
    }

    private void PickUpAmmo()
    {
        Destroy(pickUp);
        pickUp = null;
        pressE = null;
        weaponScript.IncrementAmmoCount(ammoPerBox);
    }
}
