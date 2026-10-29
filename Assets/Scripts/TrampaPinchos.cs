using UnityEngine;

public class TrampaPinchos : MonoBehaviour
{
    [Header("Configuración de Daño")]
    public int danio = 10;
    public float tiempoEntreDanio = 1.0f; // Tiempo de espera si el jugador se queda encima
    private float tiempoSiguienteDanio = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Al tocar por primera vez los pinchos
        AplicarDanio(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Si el jugador se queda parado encima de los pinchos
        if (Time.time >= tiempoSiguienteDanio)
        {
            AplicarDanio(collision);
        }
    }

    private void AplicarDanio(Collider2D collision)
    {
        // Busca el script PlayerHealth en el objeto que colisionó o en sus componentes padres
        PlayerHealth jugador = collision.GetComponent<PlayerHealth>();
        if (jugador == null)
        {
            jugador = collision.GetComponentInParent<PlayerHealth>();
        }

        if (jugador != null)
        {
            jugador.TakeDamage(danio);
            tiempoSiguienteDanio = Time.time + tiempoEntreDanio;
        }
    }
}