using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject[] meteorPrefabs;   // Array instead of single prefab
    public float spawnRate = 5f;
    public float switchInterval = 20f;   // Time before switching to next prefab

    private Camera mainCamera;

    public float horizontalSpawnPadding = 0.5f;

    private int currentIndex = 0;
    private float timer = 0f;
    private bool spawningActive = true;

    void Start()
    {
        mainCamera = Camera.main;

        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnRate);
    }

    void Update()
    {
        if (!spawningActive)
            return;

        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            timer = 0f;
            currentIndex++;

            // If we reached the end → stop spawning
            if (currentIndex >= meteorPrefabs.Length)
            {
                spawningActive = false;
                CancelInvoke(nameof(SpawnMeteor));
            }
        }
    }

    void SpawnMeteor()
    {
        if (!PlayerController.IsAlive || !spawningActive)
            return;

        // Safety check
        if (meteorPrefabs.Length == 0)
            return;

        // Camera bounds
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

       float left = mainCamera.transform.position.x - cameraWidth + horizontalSpawnPadding;
float right = mainCamera.transform.position.x + cameraWidth - horizontalSpawnPadding;
        float top = mainCamera.transform.position.y + cameraHeight;

        float randomX = Random.Range(left, right);
        Vector2 spawnPos = new Vector2(randomX, top + 1f);

        Instantiate(meteorPrefabs[currentIndex], spawnPos, Quaternion.identity);
    }
}