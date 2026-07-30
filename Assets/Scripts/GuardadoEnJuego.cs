using UnityEngine;

public class GuardadoEnJuego : MonoBehaviour
{
    [Header("Referencias")]
    public SaveManager saveManager;
    public Transform transformJugador; // Para saber dónde está el jugador al darle al botón

    // Esta función se la pondremos a tu nuevo botón de "Guardar"
    public void Click_GuardarPartida()
    {
        // 1. Primero, leemos los datos que ya existen en el archivo (para no borrar su inventario o vida)
        DatosPartida datosActuales = saveManager.CargarPartida();

        // Por si acaso hubiera un error y no existiera el archivo, creamos uno en blanco
        if (datosActuales == null)
        {
            datosActuales = new DatosPartida();
        }

        // 2. Actualizamos SOLO la posición en esos datos, copiando la posición real del jugador
        datosActuales.posicionJugador = transformJugador.position;

        // 3. Sobrescribimos el archivo con la nueva información actualizada
        saveManager.GuardarPartida(datosActuales);

        Debug.Log("¡Partida guardada con éxito! El jugador está en: " + transformJugador.position);
    }
}