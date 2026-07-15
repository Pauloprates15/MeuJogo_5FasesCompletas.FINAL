using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidadeAndar = 5f;
    public float velocidadeCorrer = 9f;
    public float forcaPulo = 8f;
    private float velocidadeAtual;

    [Header("Física do Pulo Pesado")]
    public float multiplicadorCair = 2.5f;
    public float multiplicadorPuloBaixo = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Escalada")]
    public float velocidadeEscalada = 4f;
    private bool estaNaEscada = false;
    private bool escalando = false;

    [Header("Configurações de Áudio")]
    public AudioClip somMoeda;
    public AudioClip somInimigoDerrotado;


    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private PlayerHealth playerHealth;

    private bool estaNoChao;
    private float gravidadeOriginal;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();

        gravidadeOriginal = rb.gravityScale;

        velocidadeAtual = velocidadeAndar;


        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        estaNoChao = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        float movimentoH = Input.GetAxis("Horizontal");

        // CORRIDA

        if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(movimentoH) > 0.1f)
        {
            velocidadeAtual = velocidadeCorrer;
        }
        else
        {
            velocidadeAtual = velocidadeAndar;
        }

        // ESCALADA

        float movimentoV = Input.GetAxis("Vertical");


        if (estaNaEscada && Mathf.Abs(movimentoV) > 0.1f)
        {
            escalando = true;
        }


        if (escalando)
        {
            rb.gravityScale = 0;

            rb.linearVelocity = new Vector2(
                movimentoH * (velocidadeAndar * 0.5f),
                movimentoV * velocidadeEscalada
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(
                movimentoH * velocidadeAtual,
                rb.linearVelocity.y
            );

            rb.gravityScale = gravidadeOriginal;
        }

        // DIREÇÃO

        if (movimentoH > 0)
            transform.localScale = new Vector3(1, 1, 1);

        else if (movimentoH < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // PULO

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao && !escalando)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                forcaPulo
            );
        }

        // ANIMATOR

        if (animator != null)
        {
            float velocidadeAnimacao =
                Mathf.Abs(movimentoH) *
                (velocidadeAtual / velocidadeAndar);


            animator.SetFloat("Speed", velocidadeAnimacao);

            animator.SetBool(
                "estaNoChao",
                estaNoChao
            );

            animator.SetFloat(
                "yVelocity",
                rb.linearVelocity.y
            );
        }
    }

    void FixedUpdate()
    {
        if (!escalando)
        {
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity +=
                    Vector2.up *
                    Physics2D.gravity.y *
                    (multiplicadorCair - 1) *
                    Time.fixedDeltaTime;
            }


            else if (
                rb.linearVelocity.y > 0 &&
                !Input.GetKey(KeyCode.Space)
            )
            {
                rb.linearVelocity +=
                    Vector2.up *
                    Physics2D.gravity.y *
                    (multiplicadorPuloBaixo - 1) *
                    Time.fixedDeltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // ZONA DE MORTE

        if (collision.CompareTag("Morte"))
        {
            if (playerHealth != null)
            {
                playerHealth.DieInstantly();
            }

            return;
        }

        // INIMIGO

        if (collision.CompareTag("Inimigo"))
        {

            // Pulou em cima do inimigo

            if (
                rb.linearVelocity.y < -0.1f &&
                transform.position.y > collision.transform.position.y + 0.2f
            )
            {

                rb.linearVelocity =
                    new Vector2(
                        rb.linearVelocity.x,
                        forcaPulo * 0.8f
                    );


                TocarSomInimigo();


                Destroy(collision.gameObject);


                Debug.Log("Inimigo destruído!");
            }

            else
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
            }
        }

        // MOEDA

        if (collision.CompareTag("Moeda"))
        {
            TocarSomMoeda();

            Destroy(collision.gameObject);
        }

        // ESCADA

        if (collision.CompareTag("Escada"))
        {
            estaNaEscada = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Escada"))
        {
            estaNaEscada = false;

            escalando = false;

            rb.gravityScale = gravidadeOriginal;
        }
    }

    public void TocarSomMoeda()
    {
        if (somMoeda != null && audioSource != null)
        {
            audioSource.PlayOneShot(somMoeda);
        }
    }

    public void TocarSomInimigo()
    {
        if (somInimigoDerrotado != null && audioSource != null)
        {
            audioSource.PlayOneShot(somInimigoDerrotado);
        }
    }
}