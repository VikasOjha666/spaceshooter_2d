using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerController : MonoBehaviour
{
    public static bool IsAlive { get; private set; } = true;
    public static int BulletStrength { get; private set; } = 1;

    public float moveSpeed = 8f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    Animator animator;
    Collider2D col;

    bool isDead;

    void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        IsAlive = true;
        BulletStrength = 1;
    }

    public static void IncreaseBulletStrength(int amount)
    {
        if (amount > 0)
            BulletStrength += amount;
    }

    void Update()
    {
        if (isDead)
            return;

        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Meteor"))
        {
            Destroy(collision.gameObject);
            Die();
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        IsAlive = false;

        col.enabled = false;

        // Play ship explosion sound via AudioManager (preloaded at game start)
        AudioManager.PlayShipExplosion(transform.position);

        if (animator != null){
            animator.SetBool("enemy_col", true);
            Destroy(gameObject, 0.4f);
        }
        else
            Destroy(gameObject);
    }

   void Move()
{
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");

    Vector3 movement = new Vector3(moveX, moveY, 0f).normalized;
    transform.Translate(movement * moveSpeed * Time.deltaTime);

    ClampToCamera();
}

void ClampToCamera()
{
    Camera cam = Camera.main;

    // World-space camera bounds
    float halfWidth = cam.orthographicSize * cam.aspect;

    // Player half-width
    float playerHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;

    Vector3 pos = transform.position;
    pos.x = Mathf.Clamp(
        pos.x,
        -halfWidth + playerHalfWidth,
         halfWidth - playerHalfWidth
    );

    transform.position = pos;
}

    void Shoot()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        PlayerBullet bullet = bulletObject.GetComponent<PlayerBullet>();

        if (bullet != null)
            bullet.SetDamage(BulletStrength);
    }

    // Called from Animation Event
    public void DestroyShip()
    {
        Destroy(gameObject);
    }
}