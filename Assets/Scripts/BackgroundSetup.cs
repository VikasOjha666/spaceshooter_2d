using UnityEngine;

public class BackgroundSetup : MonoBehaviour
{
    [SerializeField] Transform background1;
    [SerializeField] Transform background2;

    void Start()
    {
        float height = background1.GetComponent<SpriteRenderer>().bounds.size.y;
        background1.position = Vector3.zero;
        background2.position = Vector3.up * height;
    }
}