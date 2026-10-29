using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public Collider2D swordHitbox; // Arrastra aquí el objeto 'HitboxEspada'

    [Header("Control de Ataque")]
    public KeyCode attackKey = KeyCode.J; // O la tecla que prefieras (Z, X, Mouse0)
    public float attackCooldown = 0.4f;
    private float nextAttackTime = 0f;
    public int attackDamage = 15; // o el daño base que tenga tu jugador

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (swordHitbox != null) swordHitbox.enabled = false;
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(attackKey) || Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Atacar"); // Dispara la animación AtacarPJ
    }

    // Métodos para Animation Events
    public void EnableSwordHitbox()
    {
        if (swordHitbox != null) swordHitbox.enabled = true;
    }

    public void DisableSwordHitbox()
    {
        if (swordHitbox != null) swordHitbox.enabled = false;
    }
}