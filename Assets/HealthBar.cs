﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public Image currentHealthbar;

    private float health = 100;
    private float maxHealth = 100;

    private void Start()
    {
        UpdateHealthbar();
    }

    private void UpdateHealthbar()
    {
        float ratio = health / maxHealth;
        currentHealthbar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    private void TakeDamage(float damage)
    {
        Debug.Log("Damage : " + damage);
        health -= damage;
        if (health < 0)
        {
            health = 0;
            Debug.Log("Dead");
        }
        UpdateHealthbar();
    }

    private void Heal(float heal)
    {
        health += heal;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateHealthbar();
    }
}
