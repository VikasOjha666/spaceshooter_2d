// Meteor.cs
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float speed = 3f;

    Rigidbody2D rb;
    Animator animator;
    bool exploding = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (exploding)
            return;

        rb.MovePosition(rb.position + Vector2.down * speed * Time.fixedDeltaTime);

        if (rb.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }

    public void Explode()
{
    exploding = true;

    animator.SetBool("blast", true);

    GetComponent<Collider2D>().enabled = false;

    rb.linearVelocity = Vector2.zero;

    Destroy(gameObject, 0.2f); // adjust to animation length
}


}