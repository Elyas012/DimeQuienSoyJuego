using UnityEngine;

public class LlaveItem : MonoBehaviour
{
    [Header("Efecto opcional")]
    public float velocidadFlotacion = 3f;
    public float alturaFlotacion = 0.15f;
    private Vector3 posInicial;

    void Start()
    {
        posInicial = transform.position;
    }

    void Update()
    {
        // Pequeño movimiento flotante para destacar la llave
        float nuevoY = posInicial.y + Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;
        transform.position = new Vector3(posInicial.x, nuevoY, posInicial.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.name.Contains("PJ1_4"))
        {
            // Registrar la llave en el script de la princesa
            RescatePrincesa rescate = FindFirstObjectByType<RescatePrincesa>();
            if (rescate != null)
            {
                rescate.ObtenerLlave();
            }

            Debug.Log("<color=yellow>¡Has recogido la llave mística! Ahora puedes liberar a la princesa.</color>");
            Destroy(gameObject);
        }
    }
}