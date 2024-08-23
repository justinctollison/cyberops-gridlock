using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;

    [SerializeField] private int _health = 100;

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;

        if (_health < 0)
        {
            _health = 0;
        }

        if (_health == 0)
        {
            Die();
        }

        Debug.Log($"{_health}");
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
