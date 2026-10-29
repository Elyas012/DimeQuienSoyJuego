using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 6f;
    [SerializeField] private float fuerzaSalto = 10f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform detectorSuelo;
    [SerializeField] private float radioDeteccion = 0.2f;
    [SerializeField] private LayerMask capaSuelo;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // Referencia al sprite
    private float movimientoHorizontal;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtenemos el SpriteRenderer
    }

    void Update()
    {
        // 1. Movimiento horizontal
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // Volteo suave del sprite sin afectar la escala del Transform ni a sus hijos
        if (movimientoHorizontal > 0.05f)
        {
            spriteRenderer.flipX = false; // Mirar a la derecha
        }
        else if (movimientoHorizontal < -0.05f)
        {
            spriteRenderer.flipX = true;  // Mirar a la izquierda
        }

        // 2. Detección física del suelo
        if (detectorSuelo != null)
        {
            enSuelo = Physics2D.OverlapCircle(detectorSuelo.position, radioDeteccion, capaSuelo);
        }
        else
        {
            enSuelo = Mathf.Abs(rb.linearVelocity.y) < 0.01f;
        }

        // 3. Salto con ESPACIO
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // 4. Ataque con Z
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Atacar");
        }

        // 5. Actualizar parámetros en el Animator
        if (animator != null)
        {
            animator.SetFloat("Velocidad", Mathf.Abs(movimientoHorizontal));
            animator.SetBool("EnSuelo", enSuelo);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectorSuelo.position, radioDeteccion);
        }
    }
}