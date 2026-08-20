using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escenas

public class VolverAlMenu : MonoBehaviour
{
    [Header("Nombre exacto de tu escena del menú")]
    public string nombreEscenaMenu = "MenuPrincipal";

    public void BotonSalir()
    {
        // 1. Descongelamos el tiempo (por si el juego estaba pausado)
        Time.timeScale = 1f;

        // 2. Cargamos la escena del menú
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}