using UnityEngine;

public class ScrolingObject : MonoBehaviour
{
    public float speed = 5f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }


    void Update()
    {
        if (gameManager != null && gameManager.IsGameOver)
            return;

        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World );

        if (transform.position.x < -25f)
        {
            gameObject.SetActive(false);
        }
    }
}
