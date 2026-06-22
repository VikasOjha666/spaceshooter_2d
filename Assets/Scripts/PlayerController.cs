using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerController : MonoBehaviour
{
    public static bool IsAlive { get; private set; } = true;

    public float moveSpeed = 8f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    bool isDead;

    void OnEnable()
    {
        IsAlive = true;
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

#if UNITY_EDITOR
        if (Selection.activeGameObject == gameObject)
            Selection.activeGameObject = null;
#endif

        Destroy(gameObject);
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveX, moveY).normalized;

        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );
    }
}