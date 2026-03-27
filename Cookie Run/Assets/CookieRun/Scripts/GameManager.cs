using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUi;

    public bool IsGameOver { get; private set; }
    
    public void Awake()
    {
        gameOverUi.SetActive(false);
    }

    
    void Update()
    {
        if (IsGameOver && Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    public void OnPlayerDead()
    {
        IsGameOver = true;
        gameOverUi.SetActive(true);
    }
}
