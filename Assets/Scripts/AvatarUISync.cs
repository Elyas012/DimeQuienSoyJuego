using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AvatarUISync : MonoBehaviour
{
    [Header("Referencia al objeto animado")]
    [Tooltip("Arrastra aquí el objeto PJ4_0 que tiene el Animator y SpriteRenderer")]
    public SpriteRenderer spriteFuente;

    private Image imagenUI;

    void Awake()
    {
        imagenUI = GetComponent<Image>();
    }

    void LateUpdate()
    {
        if (spriteFuente != null && imagenUI != null)
        {
            // Sincroniza cada frame el sprite del Animator con la imagen de la UI
            imagenUI.sprite = spriteFuente.sprite;
        }
    }
}