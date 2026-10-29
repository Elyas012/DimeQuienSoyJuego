using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransicionFondos : MonoBehaviour
{
    [Header("Imágenes del Canvas")]
    [SerializeField] private Image imagenFondoA;
    [SerializeField] private Image imagenFondoB;
    [SerializeField] private CanvasGroup canvasGroupB;

    [Header("Lista de Fondos a rotar")]
    [SerializeField] private Sprite[] listaFondos;

    [Header("Tiempos")]
    [SerializeField] private float tiempoEspera = 6f;      // Segundos que dura cada imagen fija
    [SerializeField] private float duracionFundido = 2f;    // Tiempo que tarda en aparecer la siguiente

    private int indiceActual = 0;

    void Start()
    {
        if (listaFondos.Length < 2) return;

        // Inicializamos Fondo A con la primera imagen
        imagenFondoA.sprite = listaFondos[0];
        canvasGroupB.alpha = 0f;

        StartCoroutine(CicloFondos());
    }

    private IEnumerator CicloFondos()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEspera);

            // Siguiente índice en la lista circular
            int siguienteIndice = (indiceActual + 1) % listaFondos.Length;
            imagenFondoB.sprite = listaFondos[siguienteIndice];

            // Fundido de entrada de Fondo B (de 0 a 1 de transparencia)
            float t = 0f;
            while (t < duracionFundido)
            {
                t += Time.deltaTime;
                canvasGroupB.alpha = Mathf.Clamp01(t / duracionFundido);
                yield return null;
            }

            // Fondo A toma la imagen actual y Fondo B se vuelve transparente de nuevo
            imagenFondoA.sprite = listaFondos[siguienteIndice];
            canvasGroupB.alpha = 0f;
            indiceActual = siguienteIndice;
        }
    }
}