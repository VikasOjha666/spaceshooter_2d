using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnRate = 3f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnRate);
    }

    void SpawnMeteor()
    {
        if (!PlayerController.IsAlive)
            return;

        // Calculate camera bounds
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float left = mainCamera.transform.position.x - cameraWidth;
        float right = mainCamera.transform.position.x + cameraWidth;
        float top = mainCamera.transform.position.y + cameraHeight;

        // Spawn slightly above the visible screen
        float randomX = Random.Range(left, right);
        Vector2 spawnPos = new Vector2(randomX, top + 1f);

        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }
}