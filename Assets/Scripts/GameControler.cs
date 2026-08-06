using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Creamos un evento al que otros scripts podrán suscribirse
    public static event Action<bool> AlPausarJuego;

    [Header("Referencias de UI y Cámara")]
    [SerializeField] private GameObject objetoMirilla; // Arrastra aquí tu imagen de la mirilla
    [SerializeField] private JugadorCamara jugadorCamara; // Arrastra aquí el script JugadorCamara de tu personaje

    private bool juegoPausado = false;

    private void Start()
    {
        // Al empezar, bloqueamos el ratón y el tiempo fluye normal
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        // Nos aseguramos de que la mirilla empiece apagada por defecto
        if (objetoMirilla != null) objetoMirilla.SetActive(false);
    }

    private void Update()
    {
        // Detecta la tecla de pausa (ESC) con tu InputManager
        if (InputManager.Instancia.controles.Jugador.Pausa.WasPressedThisFrame())
        {
            if (juegoPausado) ReanudarJuego();
            else PausarJuego();
        }

        // Comprobamos la visibilidad de la mirilla solo si el juego NO está pausado
        if (!juegoPausado)
        {
            ActualizarVisibilidadMirilla();
        }
    }

    private void ActualizarVisibilidadMirilla()
    {
        if (objetoMirilla == null || jugadorCamara == null) return;

        // Si el estado de la cámara es 0 (Primera Persona), encendemos la mirilla. Si es 1 o 2, la apagamos.
        if (jugadorCamara.estadoCamara == 0)
        {
            if (!objetoMirilla.activeSelf) objetoMirilla.SetActive(true);
        }
        else
        {
            if (objetoMirilla.activeSelf) objetoMirilla.SetActive(false);
        }
    }

    public void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Si pausamos el juego, apagamos la mirilla para que no estorbe en el menú
        if (objetoMirilla != null) objetoMirilla.SetActive(false);

        // Gritamos a los cuatro vientos: "¡El juego se ha pausado!"
        AlPausarJuego?.Invoke(true);
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Al reanudar, volvemos a encender la mirilla (si estás en primera persona)
        if (objetoMirilla != null) objetoMirilla.SetActive(true);

        // Gritamos a los cuatro vientos: "¡El juego ya NO está pausado!"
        AlPausarJuego?.Invoke(false);
    }
}