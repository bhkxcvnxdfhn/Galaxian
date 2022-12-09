using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void Update()
    {
        if(IsOutOfBounds())
        {
            ObjectPool<EnemyBullet>.Instance.Recycle(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(1);
                ObjectPool<EnemyBullet>.Instance.Recycle(this);
            }
        }
    }
}
