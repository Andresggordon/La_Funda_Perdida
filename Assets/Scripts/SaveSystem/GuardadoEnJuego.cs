using UnityEngine;

public class GuardadoEnJuego : MonoBehaviour
{
    [Header("Referencias")]
    public SaveManager saveManager;
    public InventoryManager inventoryManager;

    [Header("Elementos a Guardar")]
    public Transform transformJugador;
    public Transform transformCamara;
    public int perspectivaActual = 1;

    public void Click_GuardarPartida()
    {
        DatosPartida datosActuales = saveManager.CargarPartida();

        if (datosActuales == null)
        {
            datosActuales = new DatosPartida();
        }

        if (transformJugador != null) datosActuales.posicionJugador = transformJugador.position;
        if (transformCamara != null) datosActuales.posicionCamara = transformCamara.position;

        datosActuales.tipoPerspectiva = perspectivaActual;

        // --- GUARDAR INVENTARIO ESTRUCTURADO ---
        if (inventoryManager != null)
        {
            datosActuales.nombresObjetosInventario.Clear();

            // Recorremos los 24 slots exactos
            for (int i = 0; i < inventoryManager.slots.Length; i++)
            {
                ItemData item = inventoryManager.slots[i];
                if (item != null)
                {
                    datosActuales.nombresObjetosInventario.Add(item.name);
                }
                else
                {
                    // Guardamos un texto vacío para recordar que este slot no tiene nada
                    datosActuales.nombresObjetosInventario.Add("");
                }
            }

            datosActuales.objetosDestruidosUID = inventoryManager.objetosDestruidosUID;
        }

        saveManager.GuardarPartida(datosActuales);
        Debug.Log("¡Partida guardada con éxito! Objetos destruidos: " + datosActuales.objetosDestruidosUID.Count);
    }
}