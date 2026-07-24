//BossMeteor.cs
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
    Camera mainCamera;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            
        if (!IsFullyInsideCamera())
            return;

        exploding = true;

        animator.SetBool("blast", true);

        GetComponent<Collider2D>().enabled = false;

        rb.linearVelocity = Vector2.zero;

        // Boss meteor never spawns power-ups
        // No power-up spawning logic here

        Destroy(gameObject, 0.2f);
    }
    
private bool IsFullyInsideCamera()
    {
        if (spriteRenderer == null || mainCamera == null)
            return false;
            
        Bounds bounds = spriteRenderer.bounds;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        
        Vector3 minViewport = mainCamera.WorldToViewportPoint(min);
        Vector3 maxViewport = mainCamera.WorldToViewportPoint(max);
        
        // Check if both corners are within camera viewport and in front of camera
        if (minViewport.x >= 0 && maxViewport.x <= 1 &&
            minViewport.y >= 0 && maxViewport.y <= 1 &&
            minViewport.z > 0 && maxViewport.z > 0)
        {
            return true;
        }
        
        return false;
    }
}
