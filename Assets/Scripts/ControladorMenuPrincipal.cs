using UnityEngine;
using UnityEngine.UI; // Necesario para controlar Botones
using UnityEngine.SceneManagement; // Necesario para cargar niveles

public class ControladorMenuPrincipal : MonoBehaviour
{
    [Header("Referencias")]
    public SaveManager saveManager;
    public Button botonContinuar;

    [Header("Ajustes")]
    public string nombreEscenaJuego = "Mundo1"; // ¡IMPORTANTE! Pon aquí el nombre exacto de tu escena de juego

    private void Start()
    {
        // Al arrancar el menú, le preguntamos al SaveManager si hay archivo
        if (saveManager.ExistePartida())
        {
            // Si hay partida, encendemos el botón de continuar
            botonContinuar.interactable = true;
        }
        else
        {
            // Si no hay partida (primera vez que juega), lo apagamos
            botonContinuar.interactable = false;
        }
    }

    // Esta función se la pondremos al botón "Nueva Partida"
    public void Click_NuevaPartida()
    {
        // 1. Creamos una caja de datos completamente nueva (con 4 corazones, etc.)
        DatosPartida nuevaPartida = new DatosPartida();

        // 2. Obligamos al SaveManager a guardarla (esto borra lo que hubiera antes)
        saveManager.GuardarPartida(nuevaPartida);

        // 3. Cargamos la escena del juego
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    // Esta función se la pondremos al botón "Continuar"
    public void Click_ContinuarPartida()
    {
        // Simplemente cargamos la escena. 
        // Cuando el personaje nazca en la escena, ya le pedirá los datos al SaveManager para colocarse.
        SceneManager.LoadScene(nombreEscenaJuego);
    }
}