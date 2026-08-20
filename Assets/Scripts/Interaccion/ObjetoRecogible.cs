using UnityEngine;

// Firmamos el contrato IInteractuable
public class ObjetoRecogible : MonoBehaviour, IInteractuable
{
    [Header("¿Qué objeto es este?")]
    public ItemData datosDelObjeto; // Basado en ScriptableObjects[cite: 1, 2]

    private void Start()
    {
        // Consultamos al SaveManager al nacer[cite: 6]
        SaveManager saveManager = FindFirstObjectByType<SaveManager>();
        IdentificadorObjeto idObjeto = GetComponent<IdentificadorObjeto>();

        if (saveManager != null && idObjeto != null && !string.IsNullOrEmpty(idObjeto.idUnico))
        {
            DatosPartida datos = saveManager.CargarPartida();
            if (datos != null && datos.objetosDestruidosUID != null)
            {
                if (datos.objetosDestruidosUID.Contains(idObjeto.idUnico))
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    // El contrato nos obliga a usar este nombre exacto para la función
    public void EjecutarInteraccion(GameObject jugador)
    {
        InventoryManager inventarioJugador = jugador.GetComponent<InventoryManager>();

        if (inventarioJugador != null)
        {
            // 1. Lo metemos en el inventario
            inventarioJugador.AnadirObjeto(datosDelObjeto);

            // 2. Apuntamos nuestro ID en la lista del inventario
            IdentificadorObjeto idObjeto = GetComponent<IdentificadorObjeto>();
            if (idObjeto != null && !string.IsNullOrEmpty(idObjeto.idUnico))
            {
                if (!inventarioJugador.objetosDestruidosUID.Contains(idObjeto.idUnico))
                {
                    inventarioJugador.objetosDestruidosUID.Add(idObjeto.idUnico);
                }
            }

            // 3. Desaparece del suelo
            Destroy(gameObject);
        }
    }
}