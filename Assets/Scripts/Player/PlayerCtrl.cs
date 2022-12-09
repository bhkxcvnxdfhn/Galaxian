using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D rb;

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

        CheckCanShoot();
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(x * moveSpeed, 0);
        LimitX(-7.5f, 7.5f);
    }

    private void LimitX(float min, float max)
    {
        float clampX = Mathf.Clamp(transform.position.x, min, max);
        transform.position = new Vector2(clampX, transform.position.y);
    }

    private void Shoot()
    {
        preBullet.SetActive(false);
        bullet.transform.position = muzzle.position;
        bullet.SetActive(true);
        bulletScript.SetVelocity(gameObject, muzzle.up, shootSpeed);
    }

    private void CheckCanShoot()
    {
        if (!bulletActive && !preBullet.activeInHierarchy)
        {
            preBullet.SetActive(true);
        }
    }
}
