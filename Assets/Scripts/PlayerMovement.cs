using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D mayaRb;
    public float speed;
    public SpriteRenderer spriteRenderer;

    float input;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");

        // Flip sprite
        if (input < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (input > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Set animasi gerak
        animator.SetFloat("xVelocity", Mathf.Abs(input));
    }

    void FixedUpdate()
    {
        mayaRb.velocity = new Vector2(input * speed, mayaRb.velocity.y);
    }
}
