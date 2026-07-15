using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private int bonusPoints = 100;

    private bool levelCompleted;
    private void Start()
    {
        levelCompleted = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("FinishFlag detectou: " + other.name);

        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Player tocou a bandeira");

        GameManager.Instance.CompleteLevel();

        if (levelCompleted)
            return;


        if (!other.CompareTag("Player"))
            return;


        levelCompleted = true;

        Debug.Log("Algo tocou na bandeira: " + other.name);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("Não é Player");
            return;
        }

        Debug.Log("Player chegou na bandeira!");

        if (levelCompleted)
        {
            Debug.Log("Fase já concluída");
            return;
        }

        levelCompleted = true;

        Debug.Log("Chamando GameManager");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompleteLevel(bonusPoints);
        }
        else
        {
            Debug.LogWarning("GameManager não encontrado.");
        }

    Debug.Log("Fase concluída!");


        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompleteLevel(bonusPoints);
        }
        else
        {
            Debug.LogWarning("GameManager não encontrado.");
        }
    }
}