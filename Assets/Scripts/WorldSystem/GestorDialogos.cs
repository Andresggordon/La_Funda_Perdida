using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GestorDialogos : MonoBehaviour
{
    public static GestorDialogos Instancia;

    [Header("Referencias UI")]
    public GameObject panelUI;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoMensaje;
    public Image imagenRetrato;

    [Header("Animación y Audio")]
    public float velocidadEscritura = 0.04f;
    public AudioSource fuenteAudio; // El altavoz
    public AudioClip sonidoVoz; // El archivo de sonido "blip"
    
    [HideInInspector] public bool estaEscribiendo = false;
    private string mensajeActual = "";
    private Coroutine rutinaEscritura;

    private void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (panelUI != null) panelUI.SetActive(false);
    }

    public void MostrarMensaje(PerfilPersonaje perfil, string mensaje)
    {
        textoNombre.text = perfil.nombre;
        imagenRetrato.sprite = perfil.retrato;
        mensajeActual = mensaje;
        panelUI.SetActive(true);

        if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);
        rutinaEscritura = StartCoroutine(EscribirTexto(mensaje));
    }

    private IEnumerator EscribirTexto(string mensaje)
    {
        estaEscribiendo = true;
        textoMensaje.text = "";

        foreach (char letra in mensaje.ToCharArray())
        {
            textoMensaje.text += letra;
            
            // Reproducimos sonido si no es un espacio (para simular pausas entre palabras)
            if (letra != ' ' && fuenteAudio != null && sonidoVoz != null)
            {
                fuenteAudio.pitch = Random.Range(0.85f, 1.15f); // Magia Animal Crossing: Tono aleatorio
                fuenteAudio.PlayOneShot(sonidoVoz);
            }

            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }

        estaEscribiendo = false; // Terminó de escribir
    }

    // Función para saltar la animación de golpe
    public void CompletarTexto()
    {
        if (rutinaEscritura != null) StopCoroutine(rutinaEscritura);
        textoMensaje.text = mensajeActual;
        estaEscribiendo = false;
    }

    public void OcultarMensaje()
    {
        panelUI.SetActive(false);
    }
}