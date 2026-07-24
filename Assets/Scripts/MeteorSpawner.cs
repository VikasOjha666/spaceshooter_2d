using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject[] meteorPrefabs;
    public GameObject[] powerUpPrefabs; // Array of power-up prefabs for each meteor type

    [Header("Boss")]
    public GameObject bossMeteorPrefab;
    public GameObject bossPowerUpPrefab; // Power-up for boss meteor
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
    private bool bossSpawnScheduled = false;

    private float prefabTimer = 0f;
    private float spawnIncreaseTimer = 0f;

    private int currentMeteorsPerSpawn;

// Track which meteor types have already spawned power-ups
    private bool[] powerUpSpawnedForType;
    private int chosenSpawnEvent;
    private int currentSpawnEvent;

void Start()
    {
        mainCamera = Camera.main;

        currentMeteorsPerSpawn = defaultMeteorsPerSpawn;

        // Initialize power-up tracking array
        powerUpSpawnedForType = new bool[meteorPrefabs.Length];

        currentSpawnEvent = 0;
        chosenSpawnEvent = Random.Range(0, Mathf.CeilToInt(switchInterval / spawnRate));

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

            currentSpawnEvent = 0;
            chosenSpawnEvent = Random.Range(0, Mathf.CeilToInt(switchInterval / spawnRate));

            currentIndex++;
            Debug.Log($"Switched to meteor type {currentIndex}");

            // Finished all regular meteor types
            if (currentIndex >= meteorPrefabs.Length)
                ScheduleBossSpawn();
        }
    }

    void ScheduleBossSpawn()
    {
        if (bossSpawnScheduled)
            return;

        bossSpawnScheduled = true;
        spawningActive = false;
        CancelInvoke(nameof(SpawnMeteor));

        if (bossMeteorPrefab != null)
            Invoke(nameof(SpawnBoss), bossSpawnDelay);
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

        int chosenMeteor = -1;

        if (!powerUpSpawnedForType[currentIndex] &&
            currentSpawnEvent == chosenSpawnEvent)
        {
            chosenMeteor = Random.Range(0, currentMeteorsPerSpawn);
        }

        for (int i = 0; i < currentMeteorsPerSpawn; i++)
        {
            float randomX = Random.Range(left, right);
            Vector2 spawnPos = new Vector2(randomX, top + 1f);

            GameObject meteor = Instantiate(meteorPrefabs[currentIndex], spawnPos, Quaternion.identity);
            
            // Set the power-up prefab for this meteor type (only if this type hasn't spawned a power-up yet)
            Meteor meteorScript = meteor.GetComponent<Meteor>();
            if (meteorScript != null && currentIndex < powerUpPrefabs.Length)
            {
                if (!powerUpSpawnedForType[currentIndex] &&
                    currentSpawnEvent == chosenSpawnEvent &&
                    i == chosenMeteor)
                {
                    meteorScript.powerUpPrefab = powerUpPrefabs[currentIndex];
                    powerUpSpawnedForType[currentIndex] = true;
                }
                else
                {
                    meteorScript.powerUpPrefab = null;
                }
            }
        }

        currentSpawnEvent++;
    }

void SpawnBoss()
    {
        if (bossMeteorPrefab == null || mainCamera == null)
            return;

        if (!PlayerController.IsAlive)
            return;

        float cameraHeight = mainCamera.orthographicSize;
        float top = mainCamera.transform.position.y + cameraHeight;

        Vector2 bossSpawnPos = new Vector2(mainCamera.transform.position.x, top + 1f);

        GameObject boss = Instantiate(bossMeteorPrefab, bossSpawnPos, Quaternion.identity);
        
        // Set the power-up prefab for the boss - NO POWER-UPS FOR BOSS
        BossMeteor bossScript = boss.GetComponent<BossMeteor>();
        if (bossScript != null)
        {
            bossScript.powerUpPrefab = null; // No power-ups for boss meteor
        }
    }
}
