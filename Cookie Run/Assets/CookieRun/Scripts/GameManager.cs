using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUi;
    public Slider hpBar;

    public float maxHP = 100f;
    public float currentHP;
    public float hpDecreasePerSecond = 5f; // 초당 줄어들 체력

    public bool IsGameOver { get; private set; }

    private PlayerController player;
    
    public void Awake()
    {
        gameOverUi.SetActive(false);

        currentHP = maxHP;

        hpBar.maxValue = maxHP;
        hpBar.value = currentHP;

        player = FindAnyObjectByType<PlayerController>();
    }

    
    void Update()
    {
        if (IsGameOver)
        {
            if (Input.GetButtonDown("Enter"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        currentHP -= hpDecreasePerSecond * Time.deltaTime;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        hpBar.value = currentHP;

        if (currentHP <= 0f)
        {
            if (player  != null)
            {
                player.DieFromHp();

            }
        }


    }


    public void OnPlayerDead()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;
        gameOverUi.SetActive(true);
    }

    public void AddHP(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);
    }

    public void DecreaseHP(float amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);
    }
}
