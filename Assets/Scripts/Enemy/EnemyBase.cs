using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Normal, Boss
}
public class EnemyBase : MonoBehaviour, IDamageable
{
    #region Reference
    private SpriteRenderer spr;
    private Animator animator;
    private Transform startParent;
    private Transform player;
    private EnemysCtrl enemysCtrl;
    private EnemysColumn enemyColumn;
    #endregion

    public int Health { get; set; } = 1;

    [SerializeField] private EnemyType enemyType = EnemyType.Normal;

    [Header("Atack")]
    [SerializeField] private float minAmplitude = 2.5f;
    [SerializeField] private float wave = 1;
    [SerializeField] private Vector2 randomTargetOffsetX = Vector2.zero;
    [SerializeField] private float shootSpeed = 10;

    [Header("Score")]
    [SerializeField] private int stayScore = 30;
    [SerializeField] private int flyScore = 60;

    private Vector3 startLoaclPos;
    private int areaNum = 0;

    private bool boss => enemyType == EnemyType.Boss;

    private bool isFlyOut;
    private bool isWaveFly;
    private bool isFlyBack;
    public bool isAttacking;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        enemysCtrl = GetComponentInParent<EnemysCtrl>();
        enemyColumn = GetComponentInParent<EnemysColumn>();
        startParent = transform.parent;
        startLoaclPos = transform.localPosition;
    }

    public void Initialization()
    {
        transform.parent = startParent;
        transform.localPosition = startLoaclPos;
        transform.rotation = Quaternion.identity;
        isAttacking = false;
        spr.flipX = false;
        spr.flipY = false;
    }

    public void Attack()
    {
        StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        isAttacking = true;
        StartCoroutine(FlyOut());
        yield return new WaitUntil(()=>!isFlyOut);
        StartCoroutine(WaveFlying());
        yield return new WaitUntil(()=>!isWaveFly);
        StartCoroutine(FlyBack());
        yield return new WaitUntil(()=>!isFlyBack);

        if (enemyColumn.GetOtherAnimationProgress() != -1)
        {
            SetAnimationProgress(enemyColumn.GetOtherAnimationProgress());
        }
        isAttacking = false;
        spr.sortingOrder = 0;
        transform.localPosition = startLoaclPos;
        enemysCtrl.EnemyAttackEnd(this);
        enemyColumn.UpdateCheckRange();
    }

    private IEnumerator FlyOut()
    {
        isFlyOut = true;
        transform.parent = null;
        animator.SetBool("Fly", true);
        spr.sortingOrder = 1;
        enemyColumn.UpdateCheckRange();

        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position + Vector3.right * areaNum * 1.5f;
        float timer = 0;

        while (timer < 1)
        {
            timer += Time.deltaTime / 0.7f;
            Vector3 point = MathfExtension.Parabola(startPos, targetPos, 0.7f, timer);
            UpdateImage(point);
            transform.position = point;
            yield return null;
        }
        isFlyOut = false;
    }

    private IEnumerator WaveFlying()
    {
        isWaveFly = true;
        float timer = 0;
        int shootCount = 2;
        float shootStep = 0.3f;
        float startShootTime = 0;
        Vector2 startPos = transform.position;
        Vector2 targetPos = player.position + Vector3.right * Random.Range(randomTargetOffsetX.x, randomTargetOffsetX.y);
        float needTime = wave * 2;

        while (transform.position.y > -5.5)
        {
            timer += Time.deltaTime / needTime;
            if (timer > 0.15f && shootCount > 0 && Time.time > startShootTime + shootStep)
            {
                Shoot();
                startShootTime = Time.time;
                shootCount--;
            }
            transform.position = MathfExtension.CosWave(startPos, targetPos, minAmplitude, wave, areaNum, timer);
            UpdateImage(player.position);
            yield return null;
        }
        isWaveFly = false;
    }

    private IEnumerator FlyBack()
    {
        isFlyBack = true;
        transform.parent = startParent;
        transform.localPosition = new Vector2(startLoaclPos.x, 2f);
        UpdateImage(startParent.position + startLoaclPos);

        float duration = 1f;
        Vector2 speed = (startLoaclPos - transform.localPosition) / duration;
        while (duration > 0)
        {
            transform.localPosition += (Vector3)speed * Time.deltaTime;
            duration -= Time.deltaTime;
            if (duration < 0.5f && animator.GetBool("Fly"))
            {
                animator.SetBool("Fly", false);
                spr.flipX = false;
                spr.flipY = false;
            }
            yield return null;
        }
        isFlyBack = false;
    }

    private void UpdateImage(Vector3 target)
    {
        Vector2 dir = target - transform.position;
        float radAngle = Mathf.Atan2(dir.y, dir.x);
        int degAngle = (int)(radAngle * Mathf.Rad2Deg);

        if(degAngle < 0)
        {
            spr.flipY = false;
            if (degAngle < -90)
                spr.flipX = boss ? false : true;
            else
                spr.flipX = boss ? true : false;
        }
        else if(degAngle > 0)
        {
            spr.flipY = true;
            if (degAngle > 90)
                spr.flipX = boss ? false : true;
            else
                spr.flipX = boss ? true : false;
        }
        degAngle = MathfExtension.GetHorizontalLineAngle(degAngle);
        animator.SetFloat("Angle", Mathf.Abs(degAngle));
    }

    private void Shoot()
    {
        EnemyBullet bullet = ObjectPool<EnemyBullet>.Instance.Spawn(transform.position, Quaternion.identity);
        bullet.SetVelocity(gameObject, new Vector2(-1 * areaNum, Random.Range(-1.9f, -2.3f)), shootSpeed);
    }

    public void Damage(int amount)
    {
        Health -= amount;

        if (Health <= 0)
            Dead();
    }

    private void Dead()
    {
        if (isAttacking)
            PlayerScore.Instance.AddScore(flyScore);
        else
            PlayerScore.Instance.AddScore(stayScore);
        ObjectPool<EnemyDeadParticle>.Instance.Spawn(transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        enemysCtrl.EnemyDead(this);
        enemyColumn.UpdateCheckRange();
        Initialization();
    }

    public float GetAnimationProgress()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public void SetAnimationProgress(float t)
    {
        animator.Play("Idle", 0, t);
    }

    public void SetAreaNum(int amount)
    {
        areaNum = amount;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();

            Dead();
            damageable?.Damage(1);
        }
    }
}
