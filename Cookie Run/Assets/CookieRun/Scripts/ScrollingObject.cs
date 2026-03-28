using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private float speed = 5f;
    private float deactivateX = -20f;

    [HideInInspector] public float cycleLength;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        if (gameManager != null && gameManager.IsGameOver)
            return;

        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        if (transform.position.x < deactivateX)
        {
            transform.position += new Vector3(cycleLength, 0f, 0f);
            ResetChildren();
        }
    }

    private void ResetChildren()
    {
        Transform[] childeren = GetComponentsInChildren<Transform>(true);

        foreach (Transform child  in childeren)
        {
            if (child == transform) continue;

            child.gameObject.SetActive(true);
        }
    }
}