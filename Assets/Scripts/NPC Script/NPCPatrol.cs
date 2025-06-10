using System.Collections;
using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public Vector2[] patrolPoints;
    public float speed = 2;
    public float pauseDuration = 1.5f;

    private bool isPaused;
    private int currentPatrolIndex;
    private Vector2 target;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(SetPatrolPoint());
    }

    void Update()
    {
        if (isPaused)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector3)target - transform.position).normalized;

        // Ganti flip dengan SpriteRenderer
        if (direction.x != 0)
            spriteRenderer.flipX = direction.x < 0;

        rb.velocity = direction * speed;

        if (Vector2.Distance(transform.position, target) < .1f)
            StartCoroutine(SetPatrolPoint());
    }

    IEnumerator SetPatrolPoint()
    {
        isPaused = true;
        anim.Play("Idle");

        yield return new WaitForSeconds(pauseDuration);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        target = patrolPoints[currentPatrolIndex];
        isPaused = false;
        anim.Play("Walk");
    }
}
