using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private int platformCount = 5;
    private float startX = 0f;
    private float groundY = -1f;

    private float basePlatformWidth = 16f; // 긴 플랫폼 기준 길이

    private GameObject[] spawnedPlatforms;

    private void Start()
    {
        if (platformPrefabs == null || platformPrefabs.Length == 0)
        {
            return;
        }

        spawnedPlatforms = new GameObject[platformCount];

        float nextX = startX;
        float totalLength = 0f;

        for (int i = 0; i < platformCount; i++)
        {
            int randomIndex = Random.Range(0, platformPrefabs.Length);
            GameObject prefab = platformPrefabs[randomIndex];

            GameObject platform = Instantiate(prefab);

            float actualWidth = GetWidth(platform);
            float centerX = nextX + actualWidth * 0.5f;

            platform.transform.position = new Vector3(centerX, groundY, 0f);
            spawnedPlatforms[i] = platform;

            // 다음 플랫폼은 항상 "기준 길이"만큼 이동
            nextX += basePlatformWidth;
            totalLength += basePlatformWidth;
        }

        for (int i = 0; i < spawnedPlatforms.Length; i++)
        {
            ScrollingObject scrolling = spawnedPlatforms[i].GetComponent<ScrollingObject>();

            if (scrolling != null)
            {
                scrolling.cycleLength = totalLength;
            }
        }
    }

    private float GetWidth(GameObject obj)
    {
        BoxCollider2D col = obj.GetComponentInChildren<BoxCollider2D>();
        if (col != null)
            return col.bounds.size.x;

        SpriteRenderer sr = obj.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            return sr.bounds.size.x;

        return 1f;
    }
}