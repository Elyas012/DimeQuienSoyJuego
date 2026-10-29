using System.Collections;
using UnityEngine;
using UnityEngine.UI;          // Necesario para Slider
using TMPro;                   // Necesario para texto en pantalla (TextMeshPro)

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI (Barra y/o Número)")]
    public Slider healthSlider;           // Arrastra aquí tu Barra de Vida (Slider)
    public TextMeshProUGUI healthText;    // (Opcional) Arrastra aquí tu texto de vida ej: "100 / 100"

    [Header("Referencias")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Efecto de Golpe (Parpadeo)")]
    public Color hurtColor = Color.red;
    private Color originalColor = Color.white;
    public float duracionParpadeo = 0.12f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // Inicializar la interfaz gráfica
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        ActualizarTextoVida();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Jugador recibió daño. Vida restante: " + currentHealth);

        // Actualizar la barra y el número
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        ActualizarTextoVida();

        // Parpadeo visual en rojo
        StartCoroutine(DamageFeedbackRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ActualizarTextoVida()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }

    IEnumerator DamageFeedbackRoutine()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hurtColor;
            yield return new WaitForSeconds(duracionParpadeo);
            spriteRenderer.color = originalColor;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log(">>> [PlayerHealth] JUGADOR DERROTADO: Entrando a Die() <<<");

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // 1. Congelar físicas y desactivar colisiones
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // 2. Apagar controles y combate
        MonoBehaviour controller = GetComponent("ControladorJugador") as MonoBehaviour;
        if (controller != null) controller.enabled = false;

        PlayerCombat combat = GetComponent<PlayerCombat>();
        if (combat != null) combat.enabled = false;

        // 3. Disparar animación de derrota
        if (animator != null)
        {
            animator.SetTrigger("Muerte");
        }

        // 4. Activar Game Over
        LlamarGameOver();
    }

    void LlamarGameOver()
    {
        if (BattleManager.instance != null)
        {
            BattleManager.instance.IniciarMuerteJugador();
            return;
        }
    }
}