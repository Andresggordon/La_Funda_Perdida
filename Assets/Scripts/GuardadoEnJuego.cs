using UnityEngine;

public class GuardadoEnJuego : MonoBehaviour
{
    [Header("Referencias")]
    public SaveManager saveManager;
    public InventoryManager inventoryManager; // NUEVO: Para acceder a la lista de setas destruidas

    [Header("Elementos a Guardar")]
    public Transform transformJugador;
    public Transform transformCamara;
    public int perspectivaActual = 1;

    public void Click_GuardarPartida()
    {
        // 1. Leemos los datos que ya existen
        DatosPartida datosActuales = saveManager.CargarPartida();

        if (datosActuales == null)
        {
            datosActuales = new DatosPartida();
        }

        // 2. Actualizamos los datos del jugador y cámara
        if (transformJugador != null)
        {
            datosActuales.posicionJugador = transformJugador.position;
        }

        if (transformCamara != null)
        {
            datosActuales.posicionCamara = transformCamara.position;
        }

        datosActuales.tipoPerspectiva = perspectivaActual;

        // --- GUARDAR INVENTARIO ---
        if (inventoryManager != null)
        {
            datosActuales.nombresObjetosInventario.Clear();
            foreach (ItemData item in inventoryManager.objetosActuales)
            {
                if (item != null)
                {
                    datosActuales.nombresObjetosInventario.Add(item.name); // Guardamos el nombre del archivo ItemData
                }
            }
        }

        // --- NUEVO: GUARDAR LA LISTA DE OBJETOS DESTRUIDOS ---
        if (inventoryManager != null)
        {
            datosActuales.objetosDestruidosUID = inventoryManager.objetosDestruidosUID;
        }

        // 3. Guardamos todo en el disco duro
        saveManager.GuardarPartida(datosActuales);

        Debug.Log("¡Partida guardada con éxito! Objetos destruidos guardados: " + datosActuales.objetosDestruidosUID.Count);
    }
}