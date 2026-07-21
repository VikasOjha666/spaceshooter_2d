using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject[] meteorPrefabs;

    [Header("Boss")]
    public GameObject bossMeteorPrefab;
    public float bossSpawnDelay = 2f;

    [Header("Spawn Timing")]
    public float spawnRate = 4f;
    public float switchInterval = 20f;

    [Header("Spawn Count")]
    public int defaultMeteorsPerSpawn = 1;
    public int maxMeteorsPerSpawn = 5;
    public float spawnIncreaseInterval = 5f;

    [Header("Spawn Position")]
    public float horizontalSpawnPadding = 0.5f;

    private Camera mainCamera;

    private int currentIndex = 0;
    private bool spawningActive = true;

    private float prefabTimer = 0f;
    private float spawnIncreaseTimer = 0f;

    private int currentMeteorsPerSpawn;

    void Start()
    {
        mainCamera = Camera.main;

        currentMeteorsPerSpawn = defaultMeteorsPerSpawn;

        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnRate);
    }

    void Update()
    {
        if (!spawningActive)
            return;

        prefabTimer += Time.deltaTime;
        spawnIncreaseTimer += Time.deltaTime;

        // Gradually increase meteors spawned
        if (spawnIncreaseTimer >= spawnIncreaseInterval)
        {
            spawnIncreaseTimer = 0f;

            if (currentMeteorsPerSpawn < maxMeteorsPerSpawn)
                currentMeteorsPerSpawn++;
        }

        // Switch to next prefab
        if (prefabTimer >= switchInterval)
        {
            prefabTimer = 0f;
            spawnIncreaseTimer = 0f;

            // Reset spawn count for new prefab
            currentMeteorsPerSpawn = defaultMeteorsPerSpawn;

            currentIndex++;

            // Finished all regular meteor types
            if (currentIndex >= meteorPrefabs.Length)
            {
                spawningActive = false;
                CancelInvoke(nameof(SpawnMeteor));

                // Spawn the boss after a delay
                if (bossMeteorPrefab != null)
                    Invoke(nameof(SpawnBoss), bossSpawnDelay);
            }
        }
    }

    void SpawnMeteor()
    {
        if (!PlayerController.IsAlive || !spawningActive)
            return;

        if (meteorPrefabs.Length == 0)
            return;

        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float left = mainCamera.transform.position.x - cameraWidth + horizontalSpawnPadding;
        float right = mainCamera.transform.position.x + cameraWidth - horizontalSpawnPadding;
        float top = mainCamera.transform.position.y + cameraHeight;

        for (int i = 0; i < currentMeteorsPerSpawn; i++)
        {
            float randomX = Random.Range(left, right);
            Vector2 spawnPos = new Vector2(randomX, top + 1f);

            Instantiate(meteorPrefabs[currentIndex], spawnPos, Quaternion.identity);
        }
    }

    void SpawnBoss()
    {
        if (!PlayerController.IsAlive)
            return;

        if (bossMeteorPrefab == null)
            return;

        float cameraHeight = mainCamera.orthographicSize;
        float top = mainCamera.transform.position.y + cameraHeight;

        // Spawn boss in the center of the screen
        Vector2 bossSpawnPos = new Vector2(mainCamera.transform.position.x, top + 1f);

        Instantiate(bossMeteorPrefab, bossSpawnPos, Quaternion.identity);
    }
}