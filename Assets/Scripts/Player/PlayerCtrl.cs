using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private EnemysCtrl enemysCtrl;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Shoot")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject preBullet;
    [SerializeField] private float shootSpeed = 3f;

    private PlayerBullet bulletScript;
    private bool bulletActive => bullet.activeInHierarchy;

    private void Start()
    {
        bulletScript = bullet.GetComponent<PlayerBullet>();
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Z) && !bulletActive)
        {
                Shoot();
        }

        if(!bulletActive && !preBullet.activeInHierarchy)
        {
            preBullet.SetActive(true);
        }
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(x, y);

        rb.velocity = new Vector2(x * moveSpeed, 0);
        if (transform.position.x > 7.5f)
            transform.position = new Vector2(7.5f, -4.2f);
        else if (transform.position.x < -7.5f)
            transform.position = new Vector2(-7.5f, -4.2f);
    }

    private void Shoot()
    {
        preBullet.SetActive(false);
        bullet.transform.position = muzzle.position;
        bullet.SetActive(true);
        bulletScript.SetVelocity(gameObject, muzzle.up, shootSpeed);
    }
}
