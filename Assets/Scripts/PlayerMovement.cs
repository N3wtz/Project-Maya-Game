using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D mayaRb;
    public float speed;
    public float input;
    public SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        if(input < 0)
        {
            spriteRenderer.flipX = false;
        } else if (input > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        mayaRb.velocity = new Vector2 (input * speed, mayaRb.velocity.y);
    }
}
