using System.Collections;
using UnityEngine;

public class RescatePrincesa : MonoBehaviour
{
    [Header("Estado de la Llave")]
    public bool tieneLlave = false;

    [Header("Aviso de falta de llave (Opcional)")]
    public GameObject avisoFaltaLlave; // Texto o letrero tipo '¡Necesitas la llave!'

    private bool rescatada = false;

    void Start()
    {
        if (avisoFaltaLlave != null)
        {
            avisoFaltaLlave.SetActive(false);
        }
    }

    public void ObtenerLlave()
    {
        tieneLlave = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rescatada) return;

        if (collision.CompareTag("Player") || collision.gameObject.name.Contains("PJ1_4"))
        {
            // Si NO tiene la llave, no se activa la victoria
            if (!tieneLlave)
            {
                Debug.LogWarning("<color=orange>[Princesa] ¡El cristal está sellado! Necesitas la llave del inicio.</color>");
                if (avisoFaltaLlave != null)
                {
                    StopAllCoroutines();
                    StartCoroutine(MostrarAvisoTemporal());
                }
                return;
            }

            // SI TIENE LA LLAVE -> Proceder con la victoria
            rescatada = true;
            Debug.Log("<color=yellow>¡Princesa liberada con la llave! Activando Victoria...</color>");

            // 1. Detener al jugador
            MonoBehaviour controller = collision.GetComponent("ControladorJugador") as MonoBehaviour;
            if (controller != null) controller.enabled = false;

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // 2. Liberar cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // 3. Activar el VictoryPanel directamente
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

    IEnumerator MostrarAvisoTemporal()
    {
        avisoFaltaLlave.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        avisoFaltaLlave.SetActive(false);
    }
}