using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    public void Init(Vector2 direction)
    {
        rb.linearVelocity = direction * Speed;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Check Collisions against Characters, environment e.t.c    
    }
}
