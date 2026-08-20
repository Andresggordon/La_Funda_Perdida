using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SliderAnimado : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    [Header("Ajustes de Animación")]
    public float factorEscalaHover = 1.05f;
    public float factorEscalaAgarre = 1.08f;
    public float velocidadAnimacion = 15f;

    [Header("Ajustes de Sonido")]
    public AudioClip sonidoHover;
    [Range(0f, 1f)] public float volumenSonido = 0.5f;

    private Vector3 escalaOriginal;
    private Coroutine animacionActual;
    private AudioSource audioSource;
    private bool estaSeleccionado = false;

    private void Start()
    {
        escalaOriginal = transform.localScale;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    // --- RATÓN: Entrar ---
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Obligamos al mando a enfocarse en este slider cuando el ratón lo toca
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    // --- RATÓN: Salir ---
    public void OnPointerExit(PointerEventData eventData)
    {
        // Al quitar el ratón, soltamos el slider
        EventSystem.current.SetSelectedGameObject(null);
    }

    // --- MANDO / TECLADO: Seleccionado ---
    public void OnSelect(BaseEventData eventData)
    {
        if (!estaSeleccionado)
        {
            estaSeleccionado = true;
            ActivarEfectoHover();
        }
    }

    // --- MANDO / TECLADO: Deseleccionado ---
    public void OnDeselect(BaseEventData eventData)
    {
        if (estaSeleccionado)
        {
            estaSeleccionado = false;
            DesactivarEfecto();
        }
    }

    // --- INTERACCIÓN: Hacer clic o agarrar ---
    public void OnPointerDown(PointerEventData eventData)
    {
        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarEscala(escalaOriginal * factorEscalaAgarre));
    }

    // --- INTERACCIÓN: Soltar ---
    public void OnPointerUp(PointerEventData eventData)
    {
        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarEscala(estaSeleccionado ? escalaOriginal * factorEscalaHover : escalaOriginal));
    }

    private void ActivarEfectoHover()
    {
        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarEscala(escalaOriginal * factorEscalaHover));

        if (sonidoHover != null)
        {
            audioSource.PlayOneShot(sonidoHover, volumenSonido);
        }
    }

    private void DesactivarEfecto()
    {
        if (animacionActual != null) StopCoroutine(animacionActual);
        animacionActual = StartCoroutine(AnimarEscala(escalaOriginal));
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