using UnityEngine;

public class InimigoPatrulha : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 2f;
    [Tooltip("Distância (em metros/blocos) que o inimigo vai andar para cada lado a partir de onde ele nascer.")]
    public float distanciaPatrulha = 3f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool indoParaDireita = true;

    // Variáveis internas para guardar a posição real dos limites no mapa
    private float limiteEsquerdo;
    private float limiteDireito;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // O SEGREDO: Salva a posição onde você colocou o inimigo no mapa 
        // e calcula os limites matematicamente a partir dali!
        limiteEsquerdo = transform.position.x - distanciaPatrulha;
        limiteDireito = transform.position.x + distanciaPatrulha;

        // Ajuste inicial para garantir que ele comece olhando para o lado certo (Direita)
        AjustarDirecaoVisual();
    }

    void Update()
    {
        // Se estiver indo para a direita e passar do limite calculado
        if (indoParaDireita && transform.position.x >= limiteDireito)
        {
            indoParaDireita = false;
            AjustarDirecaoVisual();
        }
        // Se estiver indo para a esquerda e passar do limite calculado
        else if (!indoParaDireita && transform.position.x <= limiteEsquerdo)
        {
            indoParaDireita = true;
            AjustarDirecaoVisual();
        }
    }

    void FixedUpdate()
    {
        // Define a velocidade constante na horizontal baseada na direção usando o rb.linearVelocity que você já usava
        float velocidadeAtual = indoParaDireita ? velocidade : -velocidade;
        rb.linearVelocity = new Vector2(velocidadeAtual, rb.linearVelocity.y);
    }

    void AjustarDirecaoVisual()
    {
        if (spriteRenderer == null) return;

        // Mantida a sua lógica exata: flipX ativo para Direita, desativado para Esquerda
        if (indoParaDireita)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    // Desenha as linhas e bolinhas vermelhas automaticamente no mapa para te ajudar!
    void OnDrawGizmos()
    {
        // Se o jogo estiver rodando, desenha com base nos limites fixos calculados no Start
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(limiteEsquerdo, transform.position.y, 0), new Vector3(limiteDireito, transform.position.y, 0));
            Gizmos.DrawSphere(new Vector3(limiteEsquerdo, transform.position.y, 0), 0.1f);
            Gizmos.DrawSphere(new Vector3(limiteDireito, transform.position.y, 0), 0.1f);
        }
        // Se o jogo estiver pausado/editando, mostra a prévia de onde os limites vão ficar ao redor do inimigo
        else
        {
            Gizmos.color = Color.red;
            float previewEsquerdo = transform.position.x - distanciaPatrulha;
            float previewDireito = transform.position.x + distanciaPatrulha;

            Gizmos.DrawLine(new Vector3(previewEsquerdo, transform.position.y, 0), new Vector3(previewDireito, transform.position.y, 0));
            Gizmos.DrawSphere(new Vector3(previewEsquerdo, transform.position.y, 0), 0.1f);
            Gizmos.DrawSphere(new Vector3(previewDireito, transform.position.y, 0), 0.1f);
        }
    }
}