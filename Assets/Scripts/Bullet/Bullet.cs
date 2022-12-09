using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected GameObject spawner;
    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(GameObject spawner, Vector3 direction, float speed)
    {
        this.spawner = spawner;
        direction.Normalize();
        rb.velocity = direction * speed;
    }

    protected bool IsOutOfBounds()
    {
        return  transform.position.y <= -5.5f || transform.position.y >= 5.5f;
    }
}
