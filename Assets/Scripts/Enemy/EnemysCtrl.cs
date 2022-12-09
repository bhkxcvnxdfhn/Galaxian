using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemysCtrl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private Vector2 xRange;
    [SerializeField] private Vector2 attackSec = new Vector2(7f, 10f);

    [SerializeField] private EnemysArea rightArea;
    [SerializeField] private EnemysArea leftArea;
    [SerializeField] private EnemyColumn[] enemyColumn;

    private List<EnemyBase> enemys;
    private List<EnemyBase> activeEnemy;
    private List<EnemyBase> flyingEnemy;
    private int currentLeftEnemyCount => activeEnemy.Count;
    public int currentFlyingEnemyCount => flyingEnemy.Count;

    private bool canAttack;
    private bool stopMove = true;
    private int moveDir = 1;
    private float attackRandomTime
    {
        get 
        {
            float levelStrength = (GameSceneManager.Instance.currentLevel - 1) * 0.2f;
            float timeStrength = Mathf.Clamp((Time.time - GameSceneManager.Instance.levelStartTime) / 60, 0, 2);
            return Random.Range(attackSec.x, attackSec.y) - levelStrength - timeStrength;
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
        flyingEnemy = new List<EnemyBase>();
        ShowEnemys();
    }

    void Update()
    {
        if(IsOutOfBounds())
            ChangeMoveDir();

        if (!stopMove)
            Move();

        if(canAttack && GameSceneManager.Instance.GameStart)
        {
            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = attackRandomTime;
                Debug.Log(attackTimer);
            }
            else
                attackTimer -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            if (leftArea.HaveCanUseEneny(out EnemyBase enemy))
            {
                flyingEnemy.Add(enemy);
                enemy.Attack();
            }
        }
        else if (r == 1)
        {
            if (rightArea.HaveCanUseEneny(out EnemyBase enemy))
            {
                flyingEnemy.Add(enemy);
                enemy.Attack();
            }
        }
    }

    private void Move()
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
        foreach(EnemyColumn column in GetComponentsInChildren<EnemyColumn>())
        {
            column.UpdateCheckRange();
        }
    }

    private void ShowEnemys()
    {
        StartCoroutine(DoShowEnemys());
        //foreach(EnemyBase enemy in enemys)
        //{
        //    enemy.gameObject.SetActive(true);
        //}
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
        rightArea.SetAnimationNor();
        leftArea.SetAnimationNor();
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

    public void EnemyEndFly(EnemyBase enemy)
    {
        flyingEnemy.Remove(enemy);
    }

    public void EnemyDead(EnemyBase enemy)
    {
        activeEnemy.Remove(enemy);
        if(flyingEnemy.Contains(enemy))
            flyingEnemy.Remove(enemy);
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
