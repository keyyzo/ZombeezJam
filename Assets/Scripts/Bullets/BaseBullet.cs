using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [Header("Base Parameters")]
    [SerializeField] private int damage = 25;
    [SerializeField] private float speed = 10.0f;

    // Bullet components
    private Rigidbody2D rb;
    private SpriteRenderer bulletSPrite;
    private CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        bulletSPrite = GetComponent<SpriteRenderer>();
        gameObject.tag = "Bullet";
        rb.gravityScale = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(speed, 0.0f);
        
    }

    public void SpawnedBullet(float direction)
    {
        rb.velocity = new Vector2(speed * direction, 0.0f);
    }

    public float GetSpeed()
    { return speed; }

    public void SetSpriteDirection(float direction)
    {
        if (direction > 0)
        {
            bulletSPrite.flipX = false;
        }

        else if (direction < 0)
        { 
            bulletSPrite.flipX=true;
        }
    }

}
