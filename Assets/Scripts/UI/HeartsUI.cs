using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    [Header("Heart Images")]
    [SerializeField] private List<Image> heartsImages;

    [Header("Colors")]
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);

    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnHeartsChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHeartsChanged -= UpdateHearts;
    }

    private void Start()
    {
        if (playerHealth != null)
        {
            UpdateHearts(playerHealth.CurrentHearts);
        }
    }

    private void UpdateHearts(int currentHearts)
    {
        for (int i = 0; i < heartsImages.Count; i++)
        {
            if (heartsImages[i] == null)
                continue;

            heartsImages[i].color =
                i < currentHearts ? activeColor : inactiveColor;
        }
    }
}