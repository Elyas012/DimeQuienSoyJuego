using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AltarEleccion : MonoBehaviour
{
    [Header("Elementos de UI")]
    public GameObject avisoE;             // Texto 'Presiona E'
    public GameObject panelPactos;        // Panel con los 2 botones

    [Header("Configuración de Efectos")]
    public Light2D luzGlobal;             // 'Global Light 2D'
    public float intensidadLuzOscura = 0.12f;

    private bool jugadorEnRango = false;
    private bool pactoRealizado = false;
    private GameObject jugadorObj;

    void Start()
    {
        // Asegurar cursor siempre libre y visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (luzGlobal == null)
        {
            GameObject objLuz = GameObject.Find("Global Light 2D");
            if (objLuz != null) luzGlobal = objLuz.GetComponent<Light2D>();
        }

        if (avisoE != null) avisoE.SetActive(false);
        if (panelPactos != null) panelPactos.SetActive(false);
    }

    void Update()
    {
        if (jugadorEnRango && !pactoRealizado)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                AbrirMenuPactos();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pactoRealizado) return;

        if (collision.CompareTag("Player") || collision.gameObject.name.Contains("PJ1_4"))
        {
            jugadorEnRango = true;
            jugadorObj = collision.gameObject;
            if (avisoE != null) avisoE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.name.Contains("PJ1_4"))
        {
            jugadorEnRango = false;
            if (avisoE != null) avisoE.SetActive(false);
            if (panelPactos != null) panelPactos.SetActive(false);
        }
    }

    void AbrirMenuPactos()
    {
        if (avisoE != null) avisoE.SetActive(false);
        if (panelPactos != null) panelPactos.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // --- OPCIÓN 1: PACTO DE SOMBRAS ---
    public void ElegirPactoSombras()
    {
        if (pactoRealizado) return;
        AplicarAumentoDano();

        if (luzGlobal != null)
        {
            StartCoroutine(OscurecerLuzRoutine());
        }

        FinalizarPacto();
    }

    // --- OPCIÓN 2: PACTO DE SANGRE ---
    public void ElegirPactoSangre()
    {
        if (pactoRealizado) return;
        AplicarAumentoDano();

        if (jugadorObj != null)
        {
            PlayerHealth health = jugadorObj.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.currentHealth = Mathf.Max(1, health.currentHealth / 2);
            }
        }

        FinalizarPacto();
    }

    void AplicarAumentoDano()
    {
        if (jugadorObj != null)
        {
            PlayerCombat combat = jugadorObj.GetComponent<PlayerCombat>();
            if (combat != null)
            {
                combat.attackDamage *= 2;
            }
        }
    }

    void FinalizarPacto()
    {
        pactoRealizado = true;
        if (panelPactos != null) panelPactos.SetActive(false);
        if (avisoE != null) avisoE.SetActive(false);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(0.45f, 0.45f, 0.45f, 1f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator OscurecerLuzRoutine()
    {
        float timer = 0f;
        float duracion = 0.8f;
        float luzInicial = luzGlobal.intensity;

        while (timer < duracion)
        {
            timer += Time.deltaTime;
            luzGlobal.intensity = Mathf.Lerp(luzInicial, intensidadLuzOscura, timer / duracion);
            yield return null;
        }

        luzGlobal.intensity = intensidadLuzOscura;
    }
}