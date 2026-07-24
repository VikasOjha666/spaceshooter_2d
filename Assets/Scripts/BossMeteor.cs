using UnityEngine;

public class BossMeteor : MonoBehaviour
{
    public float speed = 2f;
    public int maxHealth = 2;
    public GameObject powerUpPrefab;

    int currentHealth;
    Rigidbody2D rb;
    Animator animator;
    bool exploding = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        if (exploding)
            return;

        rb.MovePosition(rb.position + Vector2.down * speed * Time.fixedDeltaTime);

        if (rb.position.y < -7f)
            Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        if (exploding || amount <= 0)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
            Explode();
    }

    public void Explode()
    {
        if (exploding)
            return;

        exploding = true;

        animator.SetBool("blast", true);

        GetComponent<Collider2D>().enabled = false;

        rb.linearVelocity = Vector2.zero;

        // Boss meteor never spawns power-ups
        // No power-up spawning logic here

        Destroy(gameObject, 0.2f);
    }
}
