using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float fallSpeed = 2f;
    public int strengthBonus = 1;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < -8f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerController.IncreaseBulletStrength(strengthBonus);
        Destroy(gameObject);
    }
}
