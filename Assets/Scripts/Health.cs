using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int maxHealth = 100;
    private int health;
    public event Action OnDie;

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health += -damage;
        health = health > 0 ? health : 0;

        if(health == 0)
            OnDie?.Invoke();
    }
}
