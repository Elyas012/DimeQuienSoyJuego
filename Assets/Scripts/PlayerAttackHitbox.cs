using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int attackDamage = 15;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si impactó al jefe
        BossCombat boss = collision.GetComponent<BossCombat>();
        if (boss != null)
        {
            boss.TakeDamage(attackDamage);
            Debug.Log("¡Golpeaste al jefe! Daño infligido: " + attackDamage);
        }
    }
}