using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Creamos un evento al que otros scripts podrán suscribirse
    public static event Action<bool> AlPausarJuego;

    private bool juegoPausado = false;

    private void Start()
    {
        // Al empezar, bloqueamos el ratón y el tiempo fluye normal
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        // Detecta la tecla de pausa (ESC) con tu InputManager
        if (InputManager.Instancia.controles.Jugador.Pausa.WasPressedThisFrame())
        {
            if (juegoPausado) ReanudarJuego();
            else PausarJuego();
        }
    }

    public void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Gritamos a los cuatro vientos: "¡El juego se ha pausado!"
        AlPausarJuego?.Invoke(true);
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Gritamos a los cuatro vientos: "¡El juego ya NO está pausado!"
        AlPausarJuego?.Invoke(false);
    }
}