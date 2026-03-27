using Unity.VisualScripting;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{

    public GameObject[] platformPrefabs;
    public int poolSize = 10;

    [SerializeField] private float startX = 0f;
    [SerializeField] private float groundY = -1f;

    private float nextSpawnX;

    private GameObject[] platformPool;
    private int currentIndex = 0;

    private GameManager gameManager;

    private void Awake()
    {
        platformPool = new GameObject[poolSize];

        for (int i = 0; i  < platformPool.Length; i++)
        {
            int randomPrefabIndex = Random.Range(0, platformPrefabs.Length);

            platformPool[i] = Instantiate(platformPrefabs[randomPrefabIndex]);
            platformPool[i].SetActive(false);
        }
    }

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        nextSpawnX = startX;
        
        for (int i = 0; i < poolSize; i++)
        {
            SpawnFromPool();
        }
    }


    void Update()
    {
        if (gameManager != null && gameManager.IsGameOver)
            return;

    }

    private void SpawnFromPool()
    {

        GameObject platform = platformPool[currentIndex];

        platform.SetActive(false);

        float width = GetPlatformWidth(platform);
        float centerX = nextSpawnX + width * 0.5f;

        platform.transform.position = new Vector3(centerX, groundY, 0f);
        platform.SetActive(true);

        nextSpawnX += width;

        currentIndex = (currentIndex + 1) % platformPool.Length;

    }

    private float GetPlatformWidth(GameObject platform)
    {
        SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();

        if (sr != null)
            return sr.bounds.size.x;

        return 1f;
    }
}
