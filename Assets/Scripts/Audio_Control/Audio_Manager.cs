using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Obtém o componente AudioSource anexado ao mesmo GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Nenhum AudioSource encontrado neste GameObject!");
        }
    }
}