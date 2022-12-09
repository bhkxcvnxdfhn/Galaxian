using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysArea : MonoBehaviour
{
    [SerializeField] private Area area = Area.Right;
    private EnemyColumn[] enemyCol;

    private void Start()
    {
        enemyCol = GetComponentsInChildren<EnemyColumn>();

        foreach(EnemyColumn enemyColumn in enemyCol)
        {
            enemyColumn.SetAreaNum((int)area);
        }
    }

    public bool HaveCanUseEneny(out EnemyBase enemy)
    {
        foreach(EnemyColumn enemyColumn in enemyCol)
        {
            if (enemyColumn.HaveCanUseEneny(out enemy))
            {
                return true;
            }
        }
        enemy = null;
        return false;
    }

    public void SetAnimationNor()
    {
        for(int i = 0; i < enemyCol.Length; i++)
        {
            if(i % 2 == 0)
                enemyCol[i].SetAnimationNor(0.5f);
        }
    }
}

public enum Area
{
    Right = 1,
    Left = -1,
}
