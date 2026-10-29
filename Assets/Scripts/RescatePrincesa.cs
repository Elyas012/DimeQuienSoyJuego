using UnityEngine;

public class RescatePrincesa : MonoBehaviour
{
    private bool rescatada = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!rescatada && (collision.CompareTag("Player") || collision.gameObject.name.Contains("PJ1_4")))
        {
            rescatada = true;
            Debug.Log("<color=yellow>¡Princesa rescatada! Activando Victoria...</color>");

            // 1. Detener al jugador
            MonoBehaviour controller = collision.GetComponent("ControladorJugador") as MonoBehaviour;
            if (controller != null) controller.enabled = false;

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // 2. Activar directamente el VictoryPanel en el Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                Transform panel = canvas.transform.Find("VictoryPanel");
                if (panel != null)
                {
                    panel.gameObject.SetActive(true);
                    Debug.Log("<color=green>[RescatePrincesa] VictoryPanel encendido con éxito.</color>");
                    return;
                }
            }

            // Respaldo vía BattleManager
            if (BattleManager.instance != null)
            {
                BattleManager.instance.IniciarVictoriaJefe();
            }
        }
    }
}