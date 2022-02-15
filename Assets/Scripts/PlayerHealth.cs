using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int health;
    public int maxHealth;
    public Slider playerHealthSlider;

    public void Hit(int damage)
    {
        health -= damage;
        playerHealthSlider.value = (float) health/ maxHealth;
        
        if (health <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        
    }
}
