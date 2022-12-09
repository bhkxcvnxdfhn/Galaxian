using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysArea : MonoBehaviour
{
    [SerializeField] private Area area = Area.Right;
    private EnemysColumn[] enemyCol;

    private void Start()
    {
        enemyCol = GetComponentsInChildren<EnemysColumn>();
        SetEnemyAreaNum();
    }

    private void SetEnemyAreaNum()
    {
        foreach (EnemysColumn enemyColumn in enemyCol)
        {
            enemyColumn.SetAreaNum((int)area);
        }
    }

    public bool HaveCanUseEneny(out EnemyBase enemy)
    {
        foreach(EnemysColumn enemyColumn in enemyCol)
        {
            if (enemyColumn.HaveCanUseEneny(out enemy))
            {
                return true;
            }
        }
        enemy = null;
        return false;
    }

    public void SetAnimationProgress()
    {
        for(int i = 0; i < enemyCol.Length; i++)
        {
            if(i % 2 == 0)
                enemyCol[i].SetAnimationProgress(0.5f);
        }
    }
}

public enum Area
{
    Right = 1,
    Left = -1,
}
