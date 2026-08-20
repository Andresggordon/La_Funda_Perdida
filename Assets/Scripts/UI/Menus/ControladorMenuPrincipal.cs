using UnityEngine;
using UnityEngine.UI; // Necesario para controlar Botones
using UnityEngine.SceneManagement; // Necesario para cargar niveles

public class ControladorMenuPrincipal : MonoBehaviour
{
    [Header("Referencias a Scripts y Botones")]
    public SaveManager saveManager;
    public Button botonContinuar;

    [Header("Referencias a Paneles")]
    public GameObject panelPartida;
    public GameObject panelConfirmacion;

    [Header("Ajustes")]
    public string nombreEscenaJuego = "Mundo1"; // ¡IMPORTANTE! Pon aquí el nombre exacto de tu escena de juego

    private void Start()
    {
        // Al arrancar el menú, le preguntamos al SaveManager si hay archivo
        if (saveManager.ExistePartida())
        {
            botonContinuar.interactable = true;
        }
        else
        {
            botonContinuar.interactable = false;
        }
    }

    // Esta función se la pondremos al botón "Nueva Partida" del PanelPartida
    public void Click_NuevaPartida()
    {
        // Evaluamos la situación antes de hacer nada irreversible
        if (saveManager.ExistePartida())
        {
            // ¡Peligro! Hay partida. Apagamos el panel actual y mostramos la advertencia.
            panelPartida.SetActive(false);
            panelConfirmacion.SetActive(true);
        }
        else
        {
            // Vía libre. No hay partida previa, así que creamos una directamente.
            EjecutarCreacionDePartida();
        }
    }

    // Esta función se la pondremos al botón "SÍ" del PanelConfirmacion
    public void Click_ConfirmarBorrado()
    {
        // El jugador ha aceptado borrar su partida anterior
        EjecutarCreacionDePartida();
    }

    // Esta función se la pondremos al botón "NO" del PanelConfirmacion
    public void Click_CancelarBorrado()
    {
        // El jugador se arrepiente. Ocultamos la confirmación y volvemos a los botones
        panelConfirmacion.SetActive(false);
        panelPartida.SetActive(true);
    }

    // --- LÓGICA INTERNA ---
    // Separamos la lógica de crear la partida aquí para no escribir el mismo código dos veces
    private void EjecutarCreacionDePartida()
    {
        // 1. Creamos una caja de datos completamente nueva
        DatosPartida nuevaPartida = new DatosPartida();

        // 2. Obligamos al SaveManager a guardarla (sobrescribe la anterior)
        saveManager.GuardarPartida(nuevaPartida);

        // 3. Cargamos la escena del juego
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    // Esta función se la pondremos al botón "Continuar"
    public void Click_ContinuarPartida()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }
}