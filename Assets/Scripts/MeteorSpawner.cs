using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnRate = 1f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnRate);
    }

    void SpawnMeteor()
    {
        if (!PlayerController.IsAlive)
            return;

        float randomX = Random.Range(-8f, 8f);
        Vector2 spawnPos = new Vector2(randomX, 6f);

        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }
}