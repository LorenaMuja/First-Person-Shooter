using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    public GameObject playerCam;
    public Animator playerAnimator;
    public Weapon shotgun;
    public Weapon uzi;
    private Weapon currentWeapon;
    private float timeBetweenShotsTimer;
    private bool isUsingFirstWeapon = true;
    public int maxWeaponAmmoCount;
    private int weaponAmmoCount;
    public int maxOutsideWeaponAmmoCount;
    private int outsideWeaponAmmoCount;
    public int ammoPerReload;
    public TextMeshProUGUI ammoCountText;

    void Start()
    {
        currentWeapon = shotgun;
        weaponAmmoCount = maxWeaponAmmoCount;
        outsideWeaponAmmoCount = maxOutsideWeaponAmmoCount;
        UpdateText();
    }

    private void OnValidate()
    {
        Time.timeScale = 0;
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.timeScale == 0)
        {
            Time.timeScale = 1;
            return;
        }

        if (isUsingFirstWeapon)
        {
            if (playerAnimator.GetBool("isFiring"))
            {
                playerAnimator.SetBool("isFiring", false);
            }
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && weaponAmmoCount > 0)
            {
                Shoot();
            }
            else if(playerAnimator.GetBool("isFiring"))
            {
                playerAnimator.SetBool("isFiring", false);
            }
        }

        if (IsEnemyInTheCenterOfTheScreen(out GameObject enemy))
            enemy.GetComponent<Enemy>().ShowHealthBar();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("changed weapon");
            isUsingFirstWeapon = !isUsingFirstWeapon;
            playerAnimator.SetBool("isUsingFirstWeapon", isUsingFirstWeapon);
            shotgun.gameObject.SetActive(isUsingFirstWeapon);
            uzi.gameObject.SetActive(!isUsingFirstWeapon);
            if (isUsingFirstWeapon)
                currentWeapon = shotgun;
            else
                currentWeapon = uzi;
        }

        if (timeBetweenShotsTimer > 0)
            timeBetweenShotsTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            int spaceInGunLeft = maxWeaponAmmoCount - weaponAmmoCount;
            int maxAmmoToAdd = Mathf.Min(outsideWeaponAmmoCount, ammoPerReload);
            int ammoToExchange = Mathf.Min(spaceInGunLeft, maxAmmoToAdd);
            outsideWeaponAmmoCount -= ammoToExchange;
            weaponAmmoCount += ammoToExchange;
            UpdateText();
        }
    }

    void Shoot()
    {
        if (timeBetweenShotsTimer > 0 || weaponAmmoCount <= 0)
            return;
        weaponAmmoCount--;
        UpdateText();
        timeBetweenShotsTimer = currentWeapon.timeBetweenShots;
        playerAnimator.SetBool("isFiring", true);
        if (IsEnemyInTheCenterOfTheScreen(out GameObject enemy))
        {
            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * currentWeapon.range, Color.red, 5);         
            Debug.Log(enemy.transform.name);
            enemy.GetComponent<Enemy>().Hit(currentWeapon.damage);
            enemy.GetComponent<Animator>().enabled = false;
            enemy.GetComponent<NavMeshAgent>().enabled = false;
            enemy.GetComponent<Rigidbody>().AddForce(currentWeapon.force * (enemy.transform.position - playerAnimator.transform.position).normalized);
        }
        
    }

    private bool IsEnemyInTheCenterOfTheScreen(out GameObject enemy)
    {
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, currentWeapon.range))
        {
            enemy = hit.transform.gameObject;
            return enemy.transform.gameObject.GetComponent<Enemy>() != null;
        }
        enemy = null;
        return false;
    }

    private void UpdateText()
    {
        ammoCountText.text = "Ammo: " + weaponAmmoCount + " / " + outsideWeaponAmmoCount;
    }

   public void IncrementAmmoCount(int increment)
    {
        outsideWeaponAmmoCount = Mathf.Min(outsideWeaponAmmoCount + increment, maxOutsideWeaponAmmoCount);
        UpdateText();
    }
    
}

