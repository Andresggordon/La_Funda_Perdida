using UnityEngine;

public class CargarDatosJugador : MonoBehaviour
{
    [Header("Referencias")]
    public SaveManager saveManager;
    public ItemDatabase baseDeDatosItems;

    private void Start()
    {
        DatosPartida misDatos = saveManager.CargarPartida();

        if (misDatos != null)
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            Vector3 posicionSegura = misDatos.posicionJugador;
            posicionSegura.y += 0.5f;
            transform.position = posicionSegura;

            if (controller != null) controller.enabled = true;
            Debug.Log("¡Jugador cargado correctamente en la posición: " + transform.position + "!");
        }

        InventoryManager inventario = GetComponent<InventoryManager>();

        if (misDatos != null && inventario != null)
        {
            if (misDatos.objetosDestruidosUID != null)
            {
                inventario.objetosDestruidosUID = misDatos.objetosDestruidosUID;
            }

            if (misDatos.nombresObjetosInventario != null && baseDeDatosItems != null)
            {
                // Seguridad: Si el array no está instanciado por algún motivo, lo creamos
                if (inventario.slots == null || inventario.slots.Length != inventario.capacidadTotal)
                {
                    inventario.slots = new ItemData[inventario.capacidadTotal];
                }

                // Restauramos los objetos en su posición exacta del Grid
                for (int i = 0; i < misDatos.nombresObjetosInventario.Count; i++)
                {
                    // Evitar errores si en el futuro decides ampliar la mochila
                    if (i >= inventario.slots.Length) break;

                    string nombreItem = misDatos.nombresObjetosInventario[i];

                    if (!string.IsNullOrEmpty(nombreItem))
                    {
                        ItemData objetoCargado = baseDeDatosItems.ObtenerItemPorNombre(nombreItem);
                        inventario.slots[i] = objetoCargado;
                    }
                    else
                    {
                        inventario.slots[i] = null; // Hueco vacío
                    }
                }
            }
        }

        // --- ¡NUEVO! CARGAR LA HORA DEL MUNDO ---
        if (misDatos != null && GestorTiempoMundo.Instancia != null)
        {
            GestorTiempoMundo.Instancia.horaActual = misDatos.horaDelMundo;
            // No te preocupes por la rotación, el Update de GestorTiempoMundo la actualizará en el frame 1
        }
    }
}