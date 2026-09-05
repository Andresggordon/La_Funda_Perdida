// ==========================================
// SCRIPT: CargarDatosJugador.cs
// ==========================================
using UnityEngine;

public class CargarDatosJugador : MonoBehaviour
{
    [Header("Referencias")]
    public SaveManager saveManager;
    public ItemDatabase baseDeDatosItems;
    
    [Header("Punto de Aparición (Nueva Partida)")]
    public Transform puntoCama; // ¡NUEVO! Arrastra aquí la cama

    private void Start()
    {
        DatosPartida misDatos = saveManager.CargarPartida();

        if (misDatos != null)
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            Vector3 posicionSegura;

            // --- LÓGICA DE NUEVA PARTIDA ---
            if (misDatos.esPartidaNueva && puntoCama != null)
            {
                Debug.Log("¡Partida Nueva detectada! Llevando a Paula a la cama...");
                posicionSegura = puntoCama.position;
                
                // Marcamos que ya no es partida nueva y guardamos para el futuro
                misDatos.esPartidaNueva = false;
                misDatos.posicionJugador = posicionSegura;
                saveManager.GuardarPartida(misDatos);
            }
            else
            {
                // Si NO es partida nueva, cargamos donde guardó la última vez
                posicionSegura = misDatos.posicionJugador;
            }

            posicionSegura.y += 0.5f; // Un pequeño empujón hacia arriba para no atravesar el suelo
            transform.position = posicionSegura;

            if (controller != null) controller.enabled = true;
            Debug.Log("¡Jugador cargado correctamente en la posición: " + transform.position + "!");
        }

        // --- CARGA DE INVENTARIO ---
        InventoryManager inventario = GetComponent<InventoryManager>();

        if (misDatos != null && inventario != null)
        {
            if (misDatos.objetosDestruidosUID != null)
            {
                inventario.objetosDestruidosUID = misDatos.objetosDestruidosUID;
            }

            if (misDatos.nombresObjetosInventario != null && baseDeDatosItems != null)
            {
                if (inventario.slots == null || inventario.slots.Length != inventario.capacidadTotal)
                {
                    inventario.slots = new ItemData[inventario.capacidadTotal];
                }

                for (int i = 0; i < misDatos.nombresObjetosInventario.Count; i++)
                {
                    if (i >= inventario.slots.Length) break;

                    string nombreItem = misDatos.nombresObjetosInventario[i];

                    if (!string.IsNullOrEmpty(nombreItem))
                    {
                        ItemData objetoCargado = baseDeDatosItems.ObtenerItemPorNombre(nombreItem);
                        inventario.slots[i] = objetoCargado;
                    }
                    else
                    {
                        inventario.slots[i] = null; 
                    }
                }
            }
        }
    }
}