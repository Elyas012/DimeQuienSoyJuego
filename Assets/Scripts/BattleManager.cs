using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    [Header("Paneles UI")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Ajustes")]
    public float delayAntesDeMostrar = 0.5f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Respaldo automático si los paneles no se asignaron en el Inspector
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            if (gameOverPanel == null)
            {
                Transform go = canvas.transform.Find("GameOverPanel");
                if (go != null) gameOverPanel = go.gameObject;
            }
            if (victoryPanel == null)
            {
                Transform vic = canvas.transform.Find("VictoryPanel");
                if (vic != null) victoryPanel = vic.gameObject;
            }
        }

        // Asegurar que comiencen ocultos
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    // --- GAME OVER ---
    public void IniciarMuerteJugador()
    {
        StartCoroutine(RutinaGameOver());
    }

    IEnumerator RutinaGameOver()
    {
        yield return new WaitForSecondsRealtime(delayAntesDeMostrar);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("<color=red>[BattleManager] Panel de Game Over activado.</color>");
        }
        else
        {
            // Búsqueda directa desesperada
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                Transform panel = canvas.transform.Find("GameOverPanel");
                if (panel != null) panel.gameObject.SetActive(true);
            }
        }
    }

    // --- VICTORIA ---
    public void IniciarVictoriaJefe()
    {
        StartCoroutine(RutinaVictoria());
    }

    IEnumerator RutinaVictoria()
    {
        yield return new WaitForSecondsRealtime(delayAntesDeMostrar);

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Debug.Log("<color=green>[BattleManager] ¡Panel de Victoria activado!</color>");
        }
    }

    // --- BOTONES ---
    public void ReiniciarNivel()
    {
        Debug.Log("¡SE HIZO CLIC EN REINTENTAR!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Juego");
    }

    public void IrAlMenuPrincipal()
    {
        Debug.Log("¡SE HIZO CLIC EN SALIR!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}