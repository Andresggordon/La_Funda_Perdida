using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BotonAnimado : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    [Header("Ajustes de Animación")]
    public float factorEscala = 1.1f;
    public float velocidadAnimacion = 15f;
    public float distanciaHundimiento = 4f;

    [Header("Ajustes de Sonido")]
    public AudioClip sonidoHover;
    [Range(0f, 1f)] public float volumenSonido = 0.5f;

    private Vector3 escalaOriginal;
    private Vector3 posicionOriginal;
    private Coroutine animacionActual;
    private AudioSource audioSource;

    private void Start()
    {
        escalaOriginal = transform.localScale;
        posicionOriginal = transform.localPosition;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    // --- RATÓN: Entrar ---
    public void OnPointerEnter(PointerEventData eventData)
    {
        ActivarEfectoSeleccion();
    }

    // --- RATÓN: Salir ---
    public void OnPointerExit(PointerEventData eventData)
    {
        DesactivarEfectoSeleccion();
    }

    // --- JOYSTICK / TECLADO: Seleccionado ---
    public void OnSelect(BaseEventData eventData)
    {
        ActivarEfectoSeleccion();
    }

    // --- JOYSTICK / TECLADO: Deseleccionado ---
    public void OnDeselect(BaseEventData eventData)
    {
        DesactivarEfectoSeleccion();
    }

    // Lógica unificada para cuando el botón se ilumina/selecciona (por ratón o mando)
    private void ActivarEfectoSeleccion()
    {
        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarEscala(escalaOriginal * factorEscala));

        if (sonidoHover != null)
        {
            audioSource.PlayOneShot(sonidoHover, volumenSonido);
        }
    }

    // Lógica unificada para cuando se deselecciona
    private void DesactivarEfectoSeleccion()
    {
        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarEscala(escalaOriginal));
        transform.localPosition = posicionOriginal;
    }

    // --- CLICK 3D (Hundir) ---
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localPosition = new Vector3(posicionOriginal.x, posicionOriginal.y - distanciaHundimiento, posicionOriginal.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = posicionOriginal;
    }

    private IEnumerator AnimarEscala(Vector3 escalaDestino)
    {
        while (Vector3.Distance(transform.localScale, escalaDestino) > 0.001f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, escalaDestino, Time.deltaTime * velocidadAnimacion);
            yield return null;
        }
        transform.localScale = escalaDestino;
    }
}