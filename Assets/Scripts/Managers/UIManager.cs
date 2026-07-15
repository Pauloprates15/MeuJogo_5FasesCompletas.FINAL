using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Menus")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelCompleted += ShowVictory;
            GameManager.Instance.OnGameOver += ShowGameOver;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelCompleted -= ShowVictory;
            GameManager.Instance.OnGameOver -= ShowGameOver;
        }
    }

    // ==========================
    // MENU PRINCIPAL
    // ==========================

    public void StartGame()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadLevel(1);
        }
        else
        {
            Debug.LogWarning("LevelManager não encontrado.");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // ==========================
    // VITÓRIA
    // ==========================

    public void ShowVictory()
    {
        Debug.Log("UIManager recebeu vitória!");


        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    // ==========================
    // GAME OVER
    // ==========================

    public void ShowGameOver()
    {
        Debug.Log("UIManager recebeu Game Over!");


        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    // ==========================
    // BOTÕES VITÓRIA
    // ==========================

    public void NextLevel()
    {
        Time.timeScale = 1f;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadNextLevel();
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadMainMenu();
        }
    }

    // ==========================
    // BOTÃO GAME OVER
    // ==========================

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LoadMainMenu();
        }
    }
}