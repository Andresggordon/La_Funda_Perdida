using UnityEngine;

public class GuardadoEnJuego : MonoBehaviour
{
    [Header("Referencias")]
    public SaveManager saveManager;

    [Header("Elementos a Guardar")]
    public Transform transformJugador; // Posición actual del jugador
    public Transform transformCamara;  // Posición actual de la cámara

    // Aquí puedes enlazar la variable numérica de tu script de cámara (0, 1, 2...)
    // O puedes pasarle el valor desde donde gestiones la perspectiva.
    public int perspectivaActual = 1;

    // Esta función se la pondremos a tu botón de "Guardar"
    public void Click_GuardarPartida()
    {
        // 1. Leemos los datos que ya existen para no perder nada (inventario, corazones, etc.)
        DatosPartida datosActuales = saveManager.CargarPartida();

        // Si no hay archivo previo, creamos uno nuevo con los valores por defecto
        if (datosActuales == null)
        {
            datosActuales = new DatosPartida();
        }

        // 2. Actualizamos los datos con la información real de la escena
        if (transformJugador != null)
        {
            datosActuales.posicionJugador = transformJugador.position;
        }

        if (transformCamara != null)
        {
            datosActuales.posicionCamara = transformCamara.position;
        }

        datosActuales.tipoPerspectiva = perspectivaActual;

        // 3. Guardamos la caja actualizada en el disco duro
        saveManager.GuardarPartida(datosActuales);

        Debug.Log("¡Partida guardada con éxito! Posición jugador: " + transformJugador.position);
    }
}