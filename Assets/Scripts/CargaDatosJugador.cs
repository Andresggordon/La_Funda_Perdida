using UnityEngine;

public class CargarDatosJugador : MonoBehaviour
{
    [Header("Referencias")]
    public SaveManager saveManager;

    private void Start()
    {
        // 1. Nada más empezar el nivel, le pedimos los datos al SaveManager
        DatosPartida misDatos = saveManager.CargarPartida();

        // 2. Comprobamos que el archivo existe por si acaso
        if (misDatos != null)
        {
            // 3. ¡Aplicamos los datos!
            // Colocamos al jugador en las coordenadas exactas que diga el archivo
            transform.position = misDatos.posicionJugador;

            // (Si tuvieras ya un script de vida, aquí le dirías:)
            // miScriptDeSalud.corazones = misDatos.corazonesJugador;

            Debug.Log("¡Jugador cargado correctamente en la posición: " + transform.position + "!");
        }
        else
        {
            Debug.LogError("Error: No se encontró ningún archivo de guardado al entrar al mundo.");
        }
    }
}