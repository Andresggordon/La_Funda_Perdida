using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestorEquipamiento : MonoBehaviour
{
    [Header("Referencias de Equipamiento")]
    public Transform socketManoDerecha;

    [Header("Sistemas Conectados")]
    [Tooltip("Arrastra aquí a tu jugador (donde esté el script InventoryManager)")]
    public InventoryManager inventoryManager;
    [Tooltip("Arrastra aquí tu panel de la Hotbar (donde esté el script HotbarUI)")]
    public HotbarUI hotbarUI;
    [Tooltip("Arrastra la cámara principal para saber hacia dónde lanzar el objeto")]
    public Transform camaraJugador;

    [Header("Ajustes de Físicas")]
    public float fuerzaLanzamiento = 8f;
    public float fuerzaHaciaArriba = 3f;

    private GameObject objetoEquipadoActual;
    private ItemData datosObjetoActual;

    private void Update()
    {
        if (InputManager.Instancia != null && InputManager.Instancia.controles != null)
        {
            if (InputManager.Instancia.controles.Jugador.TirarObjeto.WasPressedThisFrame())
            {
                LanzarObjetoActual();
            }
        }
    }

    public void EquiparObjeto(ItemData nuevoItem)
    {
        if (objetoEquipadoActual != null)
        {
            Destroy(objetoEquipadoActual);
            objetoEquipadoActual = null;
        }

        datosObjetoActual = nuevoItem;

        if (nuevoItem == null || nuevoItem.prefabMundo == null) return;

        // 1. Instanciamos el modelo 3D en la mano
        objetoEquipadoActual = Instantiate(nuevoItem.prefabMundo, socketManoDerecha);

        objetoEquipadoActual.transform.localPosition = nuevoItem.offsetPosicion;
        objetoEquipadoActual.transform.localEulerAngles = nuevoItem.offsetRotacion;

        // 2. CONFIGURACIÓN COSMÉTICA (En mano):
        Rigidbody rb = objetoEquipadoActual.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Desactivamos colliders mientras se lleva en la mano
        Collider[] colliders = objetoEquipadoActual.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Apagamos scripts lógicos de mundo mientras esté equipada
        MonoBehaviour[] scriptsMundo = objetoEquipadoActual.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scriptsMundo)
        {
            if (script is ObjetoRecogible || script is ObjetoInteractuable || script is ItemFisicoFrenado)
            {
                script.enabled = false;
            }
        }
    }

    private void LanzarObjetoActual()
    {
        if (datosObjetoActual == null || datosObjetoActual.prefabMundo == null || inventoryManager == null || hotbarUI == null) return;

        // 1. Instanciamos el clon físico en la posición de la mano
        GameObject objetoLanzado = Instantiate(datosObjetoActual.prefabMundo, socketManoDerecha.position, socketManoDerecha.rotation);

        // 2. ACTIVACIÓN FÍSICA Y DE MUNDO:
        Rigidbody rb = objetoLanzado.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Collider[] colliders = objetoLanzado.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        ItemFisicoFrenado frenado = objetoLanzado.GetComponent<ItemFisicoFrenado>();
        if (frenado != null) frenado.enabled = true;

        StartCoroutine(ActivarRecogidaConRetraso(objetoLanzado));

        // Ignorar colisión física sólida con el jugador
        Collider colJugador = GetComponent<Collider>();
        foreach (Collider colObjeto in colliders)
        {
            if (colJugador != null && !colObjeto.isTrigger)
            {
                Physics.IgnoreCollision(colObjeto, colJugador);
            }
        }

        // 3. Disparar el objeto hacia adelante
        if (rb != null)
        {
            Vector3 direccionDisparo = camaraJugador != null ? camaraJugador.forward : transform.forward;
            rb.AddForce(direccionDisparo * fuerzaLanzamiento + Vector3.up * fuerzaHaciaArriba, ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)), ForceMode.Impulse);
        }

        // 4. Vaciar el slot de la Hotbar / Inventario
        int slotActivo = hotbarUI.ObtenerIndiceSlotActivo();
        inventoryManager.slots[slotActivo] = null;
    }

    private IEnumerator ActivarRecogidaConRetraso(GameObject objetoLanzado)
    {
        yield return new WaitForSeconds(0.5f);

        if (objetoLanzado != null)
        {
            ObjetoRecogible recogible = objetoLanzado.GetComponent<ObjetoRecogible>();
            if (recogible != null) recogible.enabled = true;
        }
    }

    // --- MÉTODO REQUERIDO POR HOTBARUI ---
    public ItemData ObtenerItemEquipado()
    {
        return datosObjetoActual;
    }
}