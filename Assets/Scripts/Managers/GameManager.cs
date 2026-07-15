using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configuração do Jogador")]
    [SerializeField] private int startingLives = 3;

    [Header("Estado do Jogo")]
    private int currentLives;
    private int currentScore;

    private bool levelCompleted;
    private bool gameOver;

    // Eventos para outros sistemas escutarem

    public event Action<int> OnScoreChanged;
    public event Action<int> OnLivesChanged;
    public event Action OnLevelCompleted;
    public event Action OnGameOver;

    public int Score => currentScore;
    public int Lives => currentLives;
    public bool LevelCompleted => levelCompleted;
    public bool GameOver => gameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;

        DontDestroyOnLoad(gameObject);


        InitializeGame();


        Debug.Log("GameManager criado");
    }

    private void InitializeGame()
    {
        currentLives = startingLives;
        currentScore = 0;

        levelCompleted = false;
        gameOver = false;


        OnLivesChanged?.Invoke(currentLives);
        OnScoreChanged?.Invoke(currentScore);
    }

    // ==========================
    // SISTEMA DE PONTUAÇÃO
    // ==========================

    public void AddScore(int amount)
    {
        if (gameOver)
            return;

        currentScore += amount;

        OnScoreChanged?.Invoke(currentScore);
    }

    // ==========================
    // SISTEMA DE VIDA
    // ==========================

    public void LoseLife()
    {
        if (gameOver)
            return;


        currentLives--;

        OnLivesChanged?.Invoke(currentLives);

        Debug.Log("Vida perdida. Vidas restantes: " + currentLives);

        if (currentLives <= 0)
        {
            TriggerGameOver();
        }
        else
        {
            RestartCurrentLevel();
        }
    }

    public void AddLife()
    {
        currentLives++;

        OnLivesChanged?.Invoke(currentLives);
    }

    private void RestartCurrentLevel()
    {
        levelCompleted = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // ==========================
    // FINAL DA FASE
    // ==========================

    public void CompleteLevel(int bonusPoints = 0)
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        if (bonusPoints > 0)
        {
            AddScore(bonusPoints);
        }

        Debug.Log("Fase concluída!");

        OnLevelCompleted?.Invoke();
    }

    // ==========================
    // GAME OVER
    // ==========================

    public void TriggerGameOver()
    {
        gameOver = true;

        Debug.Log("Game Over");

        OnGameOver?.Invoke();
    }

    // ==========================
    // RESET
    // ==========================

    public void ResetGame()
    {
        InitializeGame();
    }

    public void ResetLevelState()
    {
        levelCompleted = false;
        gameOver = false;
    }
}