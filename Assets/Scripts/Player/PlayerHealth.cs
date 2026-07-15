using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int> OnHeartsChanged;


    [Header("Configuração de Vida")]
    [SerializeField] private int maxHearts = 3;

    [Header("Invulnerabilidade")]
    [SerializeField] private float invulnerabilityTime = 1f;

    [Header("Efeito Visual de Invulnerabilidade")]
    [SerializeField] private Color flashColor = new Color(0.8f, 0.95f, 1f, 1f);
    [SerializeField] private float flashInterval = 0.1f;

    private int currentHearts;
    private bool isInvulnerable;

    private SpriteRenderer spriteRenderer;
    private Color normalColor;

    private void Start()
    {
        currentHearts = maxHearts;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            normalColor = spriteRenderer.color;
        }

        TriggerHeartsEvent();
    }

    // ==========================
    // DANO
    // ==========================

    public void TakeDamage(int damageAmount = 1)
    {
        if (isInvulnerable)
            return;

        currentHearts -= damageAmount;

        if (currentHearts < 0)
            currentHearts = 0;

        TriggerHeartsEvent();

        if (currentHearts <= 0)
        {
            HandleDeath();
            return;
        }

        StartCoroutine(InvulnerabilityRoutine());
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float timer = 0f;

        while (timer < invulnerabilityTime)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = flashColor;

                yield return new WaitForSeconds(flashInterval);


                spriteRenderer.color = normalColor;

                yield return new WaitForSeconds(flashInterval);
            }
            else
            {
                yield return null;
            }

            timer += flashInterval * 2;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }

        isInvulnerable = false;
    }

    // ==========================
    // MORTE
    // ==========================

    private void HandleDeath()
    {
        Debug.Log("Player morreu");


        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife();
        }
        else
        {
            Debug.LogWarning("GameManager não encontrado.");
        }
    }

    // ==========================
    // ZONA DE MORTE
    // ==========================

    public void DieInstantly()
    {
        currentHearts = 0;


        TriggerHeartsEvent();


        HandleDeath();
    }

    // ==========================
    // CURA
    // ==========================

    public void Heal()
    {
        if (currentHearts < maxHearts)
        {
            currentHearts++;

            TriggerHeartsEvent();
        }
    }

    private void TriggerHeartsEvent()
    {
        OnHeartsChanged?.Invoke(currentHearts);
    }

    public int CurrentHearts => currentHearts;
}