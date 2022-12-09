using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemysCtrl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private Vector2 xRange;
    [SerializeField] private Vector2 attackRandomSec = new Vector2(7f, 10f);

    [SerializeField] private EnemysArea rightArea;
    [SerializeField] private EnemysArea leftArea;
    [SerializeField] private EnemysColumn[] enemyColumn;

    private List<EnemyBase> enemys;
    private List<EnemyBase> activeEnemy;
    private List<EnemyBase> attackingEnemy;
    private int currentLeftEnemyCount => activeEnemy.Count;
    public int currentFlyingEnemyCount => attackingEnemy.Count;

    private bool canAttack = false;
    private bool stopMove = true;
    private int moveDir = 1;
    private float attackRandomTime
    {
        get 
        {
            float levelStrength = (GameSceneManager.Instance.currentLevel - 1) * 0.2f;
            float timeStrength = Mathf.Clamp((Time.time - GameSceneManager.Instance.levelStartTime) / 60, 0, 2);
            return Random.Range(attackRandomSec.x, attackRandomSec.y) - levelStrength - timeStrength;
        }
     }
    private float attackTimer;

    private void Start()
    {
        enemys = GetComponentsInChildren<EnemyBase>().ToList();
        HideEnemys();
    }

    public void Initialization()
    {
        transform.position = new Vector3(0, 3.4f, 0);
        activeEnemy = new List<EnemyBase>(enemys);
        attackingEnemy = new List<EnemyBase>();
        ShowEnemys();
    }

    void Update()
    {
        if(IsOutOfBounds())
            ChangeMoveDir();

        if (!stopMove)
            Movement();

        if(canAttack && GameSceneManager.Instance.GameStart)
        {
            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = attackRandomTime;
            }
            else
                attackTimer -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        EnemysArea area = MathfExtension.GetRandom(leftArea, rightArea);
        if (area.HaveCanUseEneny(out EnemyBase enemy))
        {
            attackingEnemy.Add(enemy);
            enemy.Attack();
        }
    }

    private void Movement()
    {
        transform.position += Vector3.right * moveDir * Time.deltaTime * moveSpeed;
    }

    private bool IsOutOfBounds()
    {
        return (moveDir == -1 && transform.position.x <= xRange.x) || 
                (moveDir == 1 && transform.position.x >= xRange.y);
    }

    private void ChangeMoveDir()
    {
        moveDir *= -1;
    }

    private void UpdateColCheckRange()
    {
        foreach(EnemysColumn column in GetComponentsInChildren<EnemysColumn>())
        {
            column.UpdateCheckRange();
        }
    }

    private void ShowEnemys()
    {
        StartCoroutine(DoShowEnemys());
    }

    private IEnumerator DoShowEnemys()
    {
        int count = 0;
        while(count < enemyColumn.Length)
        {
            enemyColumn[count].ShowEnemy();
            count++;
            yield return new WaitForSeconds(0.1f);
        }
        InitEnemys();
        UpdateColCheckRange();
        rightArea.SetAnimationProgress();
        leftArea.SetAnimationProgress();
        attackTimer = attackRandomTime;
        stopMove = false;
    }

    public void HideEnemys()
    {
        foreach (EnemyBase enemy in enemys)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    private void InitEnemys()
    {
        foreach(EnemyBase enemy in enemys)
        {
            enemy.Initialization();
        }
    }

    public void EnemyAttackEnd(EnemyBase enemy)
    {
        attackingEnemy.Remove(enemy);
    }

    public void EnemyDead(EnemyBase enemy)
    {
        activeEnemy.Remove(enemy);
        if(attackingEnemy.Contains(enemy))
            attackingEnemy.Remove(enemy);
        if(currentLeftEnemyCount <= 0)
        {
            GameSceneManager.Instance.NextLevel();
        }
    }

    public void StopAttack()
    {
        canAttack = false;
    }
    public void StartAttack()
    {
        canAttack = true;
        attackTimer = attackRandomTime;
    }
    public void StartMove()
    {
        stopMove = false;
    }
    public void Stop()
    {
        stopMove = true;
    }
}
