using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysColumn : MonoBehaviour
{
    private EnemyBase[] colEnemys;
    private Transform[] checkRanges;

    private float checkRangeScaleX = 0.2f;
    private float colPosRightX;
    private float colPosLeftX;

    private void Start()
    {
        colEnemys = GetComponentsInChildren<EnemyBase>();
        colPosRightX = colEnemys[0].transform.localPosition.x + 0.0035f + 0.4f + checkRangeScaleX / 2;
        colPosLeftX = colEnemys[0].transform.localPosition.x - 0.0035f - 0.4f - checkRangeScaleX / 2;
        checkRanges = new Transform[2];
        checkRanges[0] = transform.GetChild(transform.childCount - 1);
        checkRanges[1] = transform.GetChild(transform.childCount - 2);
        UpdateCheckRange();
    }

    public void UpdateCheckRange()
    {
        Vector2 center = GetActiveEnemyCenterAndScaleY();
        checkRanges[0].localPosition = new Vector2(colPosRightX, center.x);
        checkRanges[1].localPosition = new Vector2(colPosLeftX, center.x);
        checkRanges[0].localScale = new Vector2(checkRangeScaleX, center.y);
        checkRanges[1].localScale = new Vector2(checkRangeScaleX, center.y);
    }

    private Vector2 GetActiveEnemyCenterAndScaleY()
    {
        int count = 0;
        float centerY = 0;
        foreach (EnemyBase enemy in colEnemys)
        {
            if (enemy.gameObject.activeInHierarchy && !enemy.isAttacking)
            {
                centerY += enemy.transform.localPosition.y;
                count++;
            }
        }

        if (count == 0)
            return Vector2.zero;
        return new Vector2(centerY / count, count * 0.5f);
    }

    public void SetAreaNum(int amount)
    {
        foreach (EnemyBase col in colEnemys)
        {
            col.SetAreaNum(amount);
        }
    }

    public bool HaveCanUseEneny(out EnemyBase canUseEnemy)
    {
        foreach(EnemyBase enemy in colEnemys)
        {
            if (enemy.gameObject.activeInHierarchy && !enemy.isAttacking)
            {
                canUseEnemy = enemy;
                return true;
            }
        }
        canUseEnemy = null;
        return false;
    }

    public float GetOtherAnimationProgress()
    {
        foreach (EnemyBase enemy in colEnemys)
        {
            if (enemy.gameObject.activeInHierarchy && !enemy.isAttacking)
            {
                return enemy.GetAnimationProgress();
            }
        }

        return -1;
    }

    public void ShowEnemy()
    {
        foreach(EnemyBase enemy in colEnemys)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    public void SetAnimationProgress(float t)
    {
        foreach(EnemyBase enemy in colEnemys)
        {
            enemy.SetAnimationProgress(t);
        }
    }
}
