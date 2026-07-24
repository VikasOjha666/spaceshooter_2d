//BulletScripts.cs
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 7f;

    [Min(1)]
    public int damage = 1;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (transform.position.y > 7f)
            Destroy(gameObject);
    }

    public void SetDamage(int value)
    {
        damage = Mathf.Max(1, value);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Meteor"))
            return;

        BossMeteor boss = collision.GetComponent<BossMeteor>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        Meteor meteor = collision.GetComponent<Meteor>();
        if (meteor != null)
        {
            meteor.Explode();
            Destroy(gameObject);
        }
    }
}
