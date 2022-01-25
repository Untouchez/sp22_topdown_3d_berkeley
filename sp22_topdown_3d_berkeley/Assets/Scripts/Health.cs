using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;

    public virtual void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    public virtual void HealDamage(int damage)
    {
        CurrentHealth += damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth,0, MaxHealth);
    }
}
