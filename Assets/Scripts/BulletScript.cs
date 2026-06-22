using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 7f;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (transform.position.y > 7f)
        {
            Destroy(gameObject);
        }
    }
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Meteor"))
    {
        Meteor meteor = collision.GetComponent<Meteor>();

        if (meteor != null)
        {
            meteor.Explode();
        }

        Destroy(gameObject);
    }
}


}