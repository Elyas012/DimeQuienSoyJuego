using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("--- Estado del Juego ---")]
    public bool juegoTerminado = false;
    public int puntuacion = 0;

    void Awake()
    {
        // Si ya había una instancia previa de otra escena, reasignamos la actual de la escena cargada
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public void TerminarJuego()
    {
        juegoTerminado = true;
        Debug.Log("Juego Terminado");
    }
}