using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerHealthUI healthUI;

    [SerializeField] private int maxHP = 3;

    public int MaxHP => maxHP;
    private int health;
    public int Health 
    {
        get { return health; }
        set 
        {
            if (value <= 0)
                health = 0;
            else
                health = value;
            OnHealthChange?.Invoke(health);
        }
    }

    public event Action<int> OnHealthChange;

    private void OnEnable()
    {
        OnHealthChange += healthUI.UpdateUI;
    }

    private void OnDestroy()
    {
        OnHealthChange -= healthUI.UpdateUI;
    }

    private void Awake()
    {
        healthUI.InitHealthUI(maxHP);
    }

    public void Initialization()
    {
        Health = maxHP;
    }

    public void Damage(int amount)
    {
        Health -= amount;
        ObjectPool<PlayerDeadParticle>.Instance.Spawn(transform.position, Quaternion.identity);

        if(Health <= 0)
        {
            gameObject.SetActive(false);
            GameSceneManager.Instance.GameOver();
        }
        else
        {
            gameObject.SetActive(false);
            GameSceneManager.Instance.RebornPlayer();
        }
    }
}
