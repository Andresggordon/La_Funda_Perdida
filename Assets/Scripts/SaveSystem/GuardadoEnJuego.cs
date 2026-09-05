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

        if (transformJugador != null)
        {
            datosActuales.posicionJugador = transformJugador.position;

            if (transformJugador.TryGetComponent(out SistemaSalud saludJugador))
                datosActuales.corazonesJugador = saludJugador.SaludActual;

            if (transformJugador.TryGetComponent(out JugadorCamara camaraJugador))
                datosActuales.tipoPerspectiva = camaraJugador.estadoCamara;
            else
                datosActuales.tipoPerspectiva = perspectivaActual;
        }
        else
        {
            datosActuales.tipoPerspectiva = perspectivaActual;
        }

        if (transformCamara != null) datosActuales.posicionCamara = transformCamara.position;

        // --- GUARDAR INVENTARIO ESTRUCTURADO ---
        if (inventoryManager != null)
        {
            datosActuales.nombresObjetosInventario.Clear();

            for (int i = 0; i < inventoryManager.slots.Length; i++)
            {
                ItemData item = inventoryManager.slots[i];
                if (item != null) datosActuales.nombresObjetosInventario.Add(item.name);
                else datosActuales.nombresObjetosInventario.Add("");
            }

            datosActuales.objetosDestruidosUID = inventoryManager.objetosDestruidosUID;
        }

        // --- ¡NUEVO! GUARDAR ESTADO DEL MUNDO (SPAWNERS) ---
        datosActuales.entidadesDerrotadasUID.Clear();

        // Buscamos todos los spawners del mapa (operación segura al guardar porque el juego suele pausarse)
        PuntoGeneracion[] todosLosSpawners = FindObjectsByType<PuntoGeneracion>(FindObjectsSortMode.None);

        foreach (PuntoGeneracion spawner in todosLosSpawners)
        {
            // Si el spawner detectó que su mob murió, guardamos su ID
            if (spawner.enemigoDerrotado)
            {
                IdentificadorObjeto idSpawner = spawner.GetComponent<IdentificadorObjeto>();
                if (idSpawner != null && !string.IsNullOrEmpty(idSpawner.idUnico))
                {
                    datosActuales.entidadesDerrotadasUID.Add(idSpawner.idUnico);
                }
            }
        }

        if (GestorTiempoMundo.Instancia != null)
            datosActuales.horaDelMundo = GestorTiempoMundo.Instancia.horaActual;

        saveManager.GuardarPartida(datosActuales);
        Debug.Log("¡Partida guardada con éxito! Entidades derrotadas: " + datosActuales.entidadesDerrotadasUID.Count);
    }
}