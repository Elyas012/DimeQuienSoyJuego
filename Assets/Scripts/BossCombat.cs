using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossCombat : MonoBehaviour
{
    [Header("Referencias Principales")]
    public Transform player;
    public Collider2D attackHitbox; // Hijo 'HitboxGarra'
    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [Header("Interfaz (UI Opcional)")]
    public Slider healthSlider;

    [Header("Estadísticas de Combate")]
    public int maxHealth = 100;
    public int currentHealth;
    public int attackDamage = 20;

    [Header("Comportamiento e IA")]
    public float attackRange = 2.5f;       // Distancia para golpear
    public float detectionRange = 7.0f;    // Distancia a la que te detecta para perseguirte
    public float moveSpeed = 2.0f;
    public float timeBetweenAttacks = 2.0f;

    [Header("Patrulla (Límites en X)")]
    public float limiteIzquierdo = 4.0f;   // Coordenada X izquierda de patrulla
    public float limiteDerecho = 15.0f;    // Coordenada X derecha de patrulla
    private bool moviendoADerecha = true;

    [Header("Efecto de Golpe")]
    public Color hurtColor = Color.red;
    private Color originalColor = Color.white;

    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null) playerObj = GameObject.Find("PJ1_4");
            if (playerObj != null) player = playerObj.transform;
        }

        if (attackHitbox == null)
        {
            attackHitbox = GetComponentInChildren<Collider2D>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }

        StartCoroutine(BossLogic());
    }

    void Update()
    {
        if (isDead || isAttacking) return;

        // Si está persiguiendo o atacando, mira al jugador
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            OrientarHacia(player.position.x);
        }
    }

    void OrientarHacia(float targetX)
    {
        if (targetX > transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator BossLogic()
    {
        while (!isDead)
        {
            if (isAttacking)
            {
                yield return null;
                continue;
            }

            float distPlayer = (player != null) ? Vector2.Distance(transform.position, player.position) : 999f;

            // 1. Si el jugador está dentro del rango de ataque -> Atacar
            if (distPlayer <= attackRange)
            {
                TriggerAttack();
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            // 2. Si el jugador está dentro de la zona de visión -> Perseguir al jugador
            else if (distPlayer <= detectionRange)
            {
                yield return StartCoroutine(ChasePlayer());
            }
            // 3. Si el jugador está lejos -> Patrullar de izquierda a derecha
            else
            {
                yield return StartCoroutine(PatrolRoutine());
            }

            yield return null;
        }
    }

    // --- RUTINA 1: PATRULLA IZQUIERDA / DERECHA ---
    IEnumerator PatrolRoutine()
    {
        if (animator != null) animator.SetBool("isWalking", true);

        float targetX = moviendoADerecha ? limiteDerecho : limiteIzquierdo;
        OrientarHacia(targetX);

        float timeout = 2.5f;
        float timer = 0f;

        while (timer < timeout && !isDead && !isAttacking)
        {
            // Si el jugador entra al rango de detección, aborta la patrulla para perseguir
            if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                break;
            }

            // Movimiento horizontal hacia el límite correspondiente
            Vector2 targetPos = new Vector2(targetX, rb.position.y);
            rb.position = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.deltaTime);

            // Llegó al límite: cambia de sentido
            if (Mathf.Abs(transform.position.x - targetX) < 0.2f)
            {
                moviendoADerecha = !moviendoADerecha;
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (animator != null) animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(0.6f); // Pausa breve al llegar al extremo
    }

    // --- RUTINA 2: PERSECUCIÓN DEL JUGADOR ---
    IEnumerator ChasePlayer()
    {
        if (animator != null) animator.SetBool("isWalking", true);

        float timer = 0f;
        while (timer < 1.2f && !isDead && !isAttacking && player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);

            if (dist <= attackRange || dist > detectionRange)
            {
                break;
            }

            Vector2 target = new Vector2(player.position.x, rb.position.y);
            rb.position = Vector2.MoveTowards(rb.position, target, moveSpeed * 1.25f * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        if (animator != null) animator.SetBool("isWalking", false);
    }

    void TriggerAttack()
    {
        isAttacking = true;
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetTrigger("Attack");
        }
    }

    // --- EVENTOS DE ANIMACIÓN ---
    public void EnableAttackHitbox()
    {
        if (attackHitbox != null) attackHitbox.enabled = true;
    }

    public void DisableAttackHitbox()
    {
        if (attackHitbox != null) attackHitbox.enabled = false;
        isAttacking = false;
    }

    // --- SISTEMA DE DAÑO Y MUERTE ---
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null) healthSlider.value = currentHealth;

        StartCoroutine(DamageFeedbackRoutine());

        if (animator != null) animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageFeedbackRoutine()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hurtColor;
            yield return new WaitForSeconds(0.12f);
            spriteRenderer.color = originalColor;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        StopAllCoroutines();

        if (animator != null) animator.SetTrigger("Die");

        if (attackHitbox != null) attackHitbox.enabled = false;

        Collider2D mainCol = GetComponent<Collider2D>();
        if (mainCol != null) mainCol.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        StartCoroutine(FadeOutBoss());
    }

    IEnumerator FadeOutBoss()
    {
        yield return new WaitForSeconds(1.8f);

        float fadeDuration = 1.5f;
        float timer = 0f;

        if (spriteRenderer != null)
        {
            Color startColor = spriteRenderer.color;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
        }

        Destroy(gameObject);
    }

    // Dibuja las líneas en la Scene para ajustar los límites fácilmente
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(limiteIzquierdo, transform.position.y - 2, 0), new Vector3(limiteIzquierdo, transform.position.y + 2, 0));
        Gizmos.DrawLine(new Vector3(limiteDerecho, transform.position.y - 2, 0), new Vector3(limiteDerecho, transform.position.y + 2, 0));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}