using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private GameObject enemyDeadParticlePrefab;
    [SerializeField] private GameObject playerDeadParticlePrefab;

    private void Start()
    {
        ObjectPool<EnemyBullet>.Instance.InitPool(enemyBulletPrefab, transform);
        ObjectPool<EnemyDeadParticle>.Instance.InitPool(enemyDeadParticlePrefab, transform);
        ObjectPool<PlayerDeadParticle>.Instance.InitPool(playerDeadParticlePrefab, transform);
    }
}
