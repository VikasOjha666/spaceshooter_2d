using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 2f;
    [SerializeField] Transform otherBackground;

    float backgroundHeight;

    void Start()
    {
        backgroundHeight =
            GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        transform.Translate(
            Vector3.down * scrollSpeed * Time.deltaTime
        );

        if (transform.position.y <= -backgroundHeight)
        {
            transform.position = new Vector3(
                transform.position.x,
                otherBackground.position.y + backgroundHeight,
                transform.position.z
            );
        }
    }
}