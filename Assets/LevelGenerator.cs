using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform player;

    public float levelWidth = 3f;
    public float minY = 0.5f;
    public float maxY = 1.5f;
    public int initialPlatforms = 10;

    private float highestY = 0f;
    private List<Transform> platforms = new List<Transform>();
    private float minHorizontalDistance = 1.2f; // minimal jarak antar platform secara horizontal

    void Start()
    {
        highestY = 0f;
        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < initialPlatforms; i++)
        {
            // Gunakan range kecil untuk platform awal
            float currentMinY = i < 5 ? 0.3f : minY;
            float currentMaxY = i < 5 ? 0.6f : maxY;

            spawnPosition.y += Random.Range(currentMinY, currentMaxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);

            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platforms.Add(platform.transform);

            highestY = spawnPosition.y;
        }
    }

    void Update()
    {
        float playerY = player.position.y;

        if (playerY + 5f > highestY) // kalau pemain mendekati batas atas, buat platform baru
        {
            Vector3 spawnPosition = Vector3.zero;
            int attempts = 0;
            bool positionValid = false;

            do
            {
                spawnPosition.y = highestY + Random.Range(minY, maxY);
                spawnPosition.x = Random.Range(-levelWidth, levelWidth);

                positionValid = true;

                // Cek apakah terlalu dekat dengan platform lain
                foreach (Transform p in platforms)
                {
                    float dx = Mathf.Abs(p.position.x - spawnPosition.x);
                    float dy = Mathf.Abs(p.position.y - spawnPosition.y);

                    if (dx < minHorizontalDistance && dy < 0.5f) // kalau terlalu dekat
                    {
                        positionValid = false;
                        break;
                    }
                }

                attempts++;
                if (attempts > 10) break; // jaga-jaga biar tidak infinite loop

            } while (!positionValid);

            GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            platforms.Add(newPlatform.transform);
            highestY = spawnPosition.y;

            // Hapus platform lama yang sudah jauh di bawah pemain
            platforms.RemoveAll(p =>
            {
                if (p == null) return true;
                if (p.position.y + 6f < playerY)
                {
                    Destroy(p.gameObject);
                    return true;
                }
                return false;
            });
        }
    }
}
