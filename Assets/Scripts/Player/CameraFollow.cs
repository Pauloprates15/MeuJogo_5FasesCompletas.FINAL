using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform alvo;
    public float suavidade = 5f;

    [Header("Configuração do Fundo")]
    public Transform background; 

    void LateUpdate()
    {
        if (alvo == null)
            return;

        // para a câmera ir atrás do jogador
        Vector3 novaPosicao = new Vector3(
            alvo.position.x,
            alvo.position.y,
            -10f
        );

        transform.position = Vector3.Lerp(
            transform.position,
            novaPosicao,
            suavidade * Time.deltaTime
        );

        // o meu background segue a camera
        if (background != null)
        {
            // Mantém o X e Y idênticos aos da câmera atual, mas trava o Z em 0 (ou 10) para aparecer na tela
            background.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }
    }
}
