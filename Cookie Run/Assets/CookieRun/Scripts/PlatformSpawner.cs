using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private int platformCount = 5;
    private float startX = 0f;
    private float groundY = -1f;

    private GameObject[] spawnedPlatforms;

    private void Start()
    {
        if (platformPrefabs == null || platformPrefabs.Length == 0)
        {
            Debug.LogError("Platform Prefabs가 비어 있습니다.");
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

            float width = GetWidth(platform);
            float centerX = nextX + width * 0.5f;

            platform.transform.position = new Vector3(centerX, groundY, 0f);
            spawnedPlatforms[i] = platform;

            nextX += width;
            totalLength += width;
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