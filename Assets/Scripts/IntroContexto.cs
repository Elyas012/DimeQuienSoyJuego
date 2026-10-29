using UnityEngine;

public class IntroContexto : MonoBehaviour
{
    [Header("Panel Principal")]
    public GameObject panelIntro;

    void Start()
    {
        // Si no se asignó en el Inspector, toma este mismo objeto
        if (panelIntro == null)
        {
            panelIntro = gameObject;
        }

        // Mostrar pantalla y pausar la acción
        panelIntro.SetActive(true);
        Time.timeScale = 0f;

        // Asegurar cursor libre para hacer clic en el botón
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // También permite avanzar presionando Espacio o Enter
        if (panelIntro.activeSelf && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            ContinuarJuego();
        }
    }

    public void ContinuarJuego()
    {
        Time.timeScale = 1f; // Reanuda físicas y animaciones del juego
        panelIntro.SetActive(false); // Oculta el panel
        Debug.Log("<color=green>[Intro] Contexto cerrado, juego iniciado.</color>");
    }
}