using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 2f;

    float backgroundHeight;

    void Start()
    {
        backgroundHeight =
            GetComponent<SpriteRenderer>().bounds.size.y;

        Debug.Log(backgroundHeight);
    }

    void Update()
    {
        transform.position +=
            Vector3.down * scrollSpeed * Time.deltaTime;

        if (transform.position.y <= -backgroundHeight)
        {
            transform.position +=
                Vector3.up * (backgroundHeight * 2f);
        }
    }
}