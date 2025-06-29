using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public Transform player;

    [Header("Generation Parameters")]
    public float levelWidth = 3f;
    public float minY = 0.5f;
    public float maxY = 1.5f;
    public int initialPlatforms = 10;
    public float platformCleanupDistance = 15f;
    public float generationTriggerDistance = 10f;

    [Header("Difficulty Scaling")]
    public bool enableDifficultyScaling = true;
    public float difficultyIncreaseRate = 0.1f;
    public float maxDifficultyMultiplier = 2f;

    private float highestY = 0f;
    private List<Transform> platforms = new List<Transform>();
    private float minHorizontalDistance = 1.2f;
    private int platformsGenerated = 0;

    void Start()
    {
        if (platformPrefab == null)
        {
            Debug.LogError("LevelGenerator: Platform prefab is not assigned!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("LevelGenerator: Player reference is not assigned!");
            return;
        }

        GenerateInitialPlatforms();
    }

    private void GenerateInitialPlatforms()
    {
        highestY = 0f;
        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < initialPlatforms; i++)
        {
            // Use smaller jumps for initial platforms to ensure accessibility
            float currentMinY = i < 5 ? 0.3f : minY;
            float currentMaxY = i < 5 ? 0.6f : maxY;

            spawnPosition.y += Random.Range(currentMinY, currentMaxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);

            CreatePlatform(spawnPosition);
            highestY = spawnPosition.y;
        }
    }

    void Update()
    {
        if (player == null) return;

        float playerY = player.position.y;

        // Generate new platforms when player approaches the top
        if (playerY + generationTriggerDistance > highestY)
        {
            GenerateNewPlatform();
        }

        // Clean up old platforms to maintain performance
        CleanupOldPlatforms(playerY);
    }

    private void GenerateNewPlatform()
    {
        Vector3 spawnPosition = GetValidPlatformPosition();
        
        if (spawnPosition != Vector3.zero)
        {
            CreatePlatform(spawnPosition);
            highestY = spawnPosition.y;
            platformsGenerated++;
        }
    }

    private Vector3 GetValidPlatformPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        int maxAttempts = 20;
        
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float difficultyMultiplier = GetDifficultyMultiplier();
            float adjustedMinY = minY * difficultyMultiplier;
            float adjustedMaxY = maxY * difficultyMultiplier;
            
            spawnPosition.y = highestY + Random.Range(adjustedMinY, adjustedMaxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);

            if (IsPositionValid(spawnPosition))
            {
                return spawnPosition;
            }
        }

        // Fallback position if no valid position found
        spawnPosition.y = highestY + (minY + maxY) / 2f;
        spawnPosition.x = 0f;
        return spawnPosition;
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Transform platform in platforms)
        {
            if (platform == null) continue;

            float dx = Mathf.Abs(platform.position.x - position.x);
            float dy = Mathf.Abs(platform.position.y - position.y);

            if (dx < minHorizontalDistance && dy < 0.5f)
            {
                return false;
            }
        }
        return true;
    }

    private float GetDifficultyMultiplier()
    {
        if (!enableDifficultyScaling) return 1f;
        
        float difficulty = 1f + (platformsGenerated * difficultyIncreaseRate);
        return Mathf.Min(difficulty, maxDifficultyMultiplier);
    }

    private void CreatePlatform(Vector3 position)
    {
        if (platformPrefab == null) return;
        
        GameObject newPlatform = Instantiate(platformPrefab, position, Quaternion.identity);
        if (newPlatform != null)
        {
            platforms.Add(newPlatform.transform);
        }
    }

    private void CleanupOldPlatforms(float playerY)
    {
        platforms.RemoveAll(platform =>
        {
            if (platform == null) return true;

            if (platform.position.y + platformCleanupDistance < playerY)
            {
                Destroy(platform.gameObject);
                return true;
            }
            return false;
        });
    }

    public void ResetGenerator()
    {
        // Clean up all existing platforms
        foreach (Transform platform in platforms)
        {
            if (platform != null)
            {
                Destroy(platform.gameObject);
            }
        }
        
        platforms.Clear();
        platformsGenerated = 0;
        GenerateInitialPlatforms();
    }

    public int GetPlatformCount()
    {
        return platforms.Count;
    }
}
