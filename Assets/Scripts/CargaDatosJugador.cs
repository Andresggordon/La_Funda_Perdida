using UnityEngine;

public class CargarDatosJugador : MonoBehaviour
{
    [Header("Referencias")]
    public SaveManager saveManager;
    public ItemDatabase baseDeDatosItems; // NUEVO: Arrastra aquí tu base de datos recién creada

    private void Start()
    {
        DatosPartida misDatos = saveManager.CargarPartida();

        if (misDatos != null)
        {
            // 1. Buscamos el Character Controller
            CharacterController controller = GetComponent<CharacterController>();

            // 2. Lo apagamos temporalmente para evitar tirones físicos
            if (controller != null)
            {
                controller.enabled = false;
            }

            // 3. Aplicamos la posición guardada
            Vector3 posicionSegura = misDatos.posicionJugador;
            posicionSegura.y += 0.5f;
            transform.position = posicionSegura;

            // 4. Lo volvemos a encender
            if (controller != null)
            {
                controller.enabled = true;
            }

            Debug.Log("¡Jugador cargado correctamente en la posición: " + transform.position + "!");
        }

        // Buscamos el inventario
        InventoryManager inventario = GetComponent<InventoryManager>();

        if (misDatos != null && inventario != null)
        {
            // --- RECUPERAR LOS OBJETOS DESTRUIDOS AL CARGAR ---
            if (misDatos.objetosDestruidosUID != null)
            {
                inventario.objetosDestruidosUID = misDatos.objetosDestruidosUID;
            }

            // --- CARGAR INVENTARIO USANDO LA BASE DE DATOS ---
            if (misDatos.nombresObjetosInventario != null && baseDeDatosItems != null)
            {
                inventario.objetosActuales.Clear();

                foreach (string nombreItem in misDatos.nombresObjetosInventario)
                {
                    // Buscamos el objeto en nuestra Base de Datos personalizada
                    ItemData objetoCargado = baseDeDatosItems.ObtenerItemPorNombre(nombreItem);

                    if (objetoCargado != null)
                    {
                        inventario.objetosActuales.Add(objetoCargado);
                    }
                }
            }
        }
    }
}