using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBullet : Bullet
{
    private bool isStop;

    private void Update()
    {
        if(IsOutOfBounds())
        {
            if (isStop)
                FindObjectOfType<EnemysCtrl>().StartMove();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();

            if(damageable != null)
            {
                damageable.Damage(1);
                gameObject.SetActive(false);
            }
            if(isStop)
                FindObjectOfType<EnemysCtrl>().StartMove();
        }
        else if(collision.CompareTag("CheckRange"))
        {
            isStop = true;
            FindObjectOfType<EnemysCtrl>().Stop();
        }
    }
}
