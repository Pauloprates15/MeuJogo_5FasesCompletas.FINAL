using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // ==========================
    // CARREGAR FASE
    // ==========================

    public void LoadLevel(int buildIndex)
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(buildIndex);
    }

    // ==========================
    // REINICIAR FASE ATUAL
    // ==========================

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // ==========================
    // PRÓXIMA FASE
    // ==========================

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;


        int nextLevel =
            SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            LoadMainMenu();
        }
    }

    // ==========================
    // MENU PRINCIPAL
    // ==========================

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    // ==========================
    // NOVO JOGO
    // ==========================

    public void NewGame()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }


        SceneManager.LoadScene(1);
    }

    // ==========================
    // SAIR DO JOGO
    // ==========================

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}