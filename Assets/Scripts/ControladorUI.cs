using System.Collections;
using UnityEngine;

public class ControladorUI : MonoBehaviour
{
    [Header("Paneles (UI)")]
    public GameObject panelPausaPrincipal;
    public GameObject panelOpciones;

    [Header("Efectos Visuales")]
    public GameObject iconoGuardado;

    // Nos suscribimos al evento del GameManager cuando este script se activa
    private void OnEnable()
    {
        GameManager.AlPausarJuego += ActualizarMenuPausa;
    }

    // Nos desuscribimos por seguridad cuando el script se desactiva
    private void OnDisable()
    {
        GameManager.AlPausarJuego -= ActualizarMenuPausa;
    }

    private void Start()
    {
        // Nos aseguramos de que los menús empiecen apagados
        if (panelPausaPrincipal != null) panelPausaPrincipal.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(false);

        // Apagamos el icono de guardado al arrancar el juego para que no moleste
        if (iconoGuardado != null) iconoGuardado.SetActive(false);
    }

    // Esta función se dispara automáticamente cuando el GameManager lanza su evento
    private void ActualizarMenuPausa(bool estaPausado)
    {
        if (estaPausado)
        {
            if (panelPausaPrincipal != null) panelPausaPrincipal.SetActive(true);
            if (panelOpciones != null) panelOpciones.SetActive(false);
        }
        else
        {
            if (panelPausaPrincipal != null) panelPausaPrincipal.SetActive(false);
            if (panelOpciones != null) panelOpciones.SetActive(false);
        }
    }

    // --- FUNCIONES PARA LOS BOTONES ---

    public void AbrirOpciones()
    {
        panelPausaPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
        panelPausaPrincipal.SetActive(true);
    }

    // Esta es la función visual que conectaremos al botón de Guardar en el Inspector
    public void MostrarEfectoGuardado()
    {
        // Iniciamos la animación de parpadeo
        StartCoroutine(EfectoParpadeoGuardado());
    }

    // Corrutina que controla el parpadeo del icono
    private IEnumerator EfectoParpadeoGuardado()
    {
        for (int i = 0; i < 3; i++)
        {
            iconoGuardado.SetActive(true);

            // Usamos WaitForSecondsRealtime porque en el menú de pausa el tiempo está a 0
            yield return new WaitForSecondsRealtime(0.3f);

            iconoGuardado.SetActive(false);

            // Usamos WaitForSecondsRealtime porque en el menú de pausa el tiempo está a 0
            yield return new WaitForSecondsRealtime(0.3f);
        }

        iconoGuardado.SetActive(false); // Aseguramos que quede apagado al terminar
    }
}