using UnityEngine;

public class GestorEquipamiento : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Arrastra aquí el Empty GameObject 'Socket_ManoDerecha' que has creado en el hueso")]
    public Transform socketManoDerecha;

    private GameObject objetoEquipadoActual; // El modelo 3D físico que estamos sosteniendo
    private ItemData datosObjetoActual;      // Los datos del objeto

    // Esta función la llamará la Hotbar cuando cambiemos de Slot
    public void EquiparObjeto(ItemData nuevoItem)
    {
        // 1. Destruimos el modelo 3D que haya en la mano actualmente
        if (objetoEquipadoActual != null)
        {
            Destroy(objetoEquipadoActual);
            objetoEquipadoActual = null;
        }

        datosObjetoActual = nuevoItem;

        // 2. Si el slot está vacío (o no tiene modelo 3D asignado), nos quedamos con la mano vacía
        if (nuevoItem == null || nuevoItem.prefabMundo == null) return;

        // 3. Instanciamos el nuevo modelo 3D como hijo del Socket de la mano
        objetoEquipadoActual = Instantiate(nuevoItem.prefabMundo, socketManoDerecha);

        // 4. Reseteamos su posición y rotación para que encaje perfecto en el anclaje
        objetoEquipadoActual.transform.localPosition = Vector3.zero;
        objetoEquipadoActual.transform.localRotation = Quaternion.identity;
    }

    public ItemData ObtenerItemEquipado()
    {
        return datosObjetoActual;
    }
}