using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestorEquipamiento : MonoBehaviour
{
    [Header("Referencias de Equipamiento")]
    public Transform socketManoDerecha;
    public Animator animatorPaula; // NUEVO: Referencia al Animator de Paula

    [Header("Sistemas Conectados")]
    public InventoryManager inventoryManager;
    public HotbarUI hotbarUI;
    public Transform camaraJugador;

    [Header("Ajustes de Físicas")]
    public float fuerzaLanzamiento = 8f;
    public float fuerzaHaciaArriba = 3f;

    private GameObject objetoEquipadoActual;
    private ItemData datosObjetoActual;
    private bool estaLanzando = false;

    private void Update()
    {
        if (estaLanzando) return; // Evitamos spam de inputs mientras lanza

        if (InputManager.Instancia != null && InputManager.Instancia.controles != null)
        {
            if (InputManager.Instancia.controles.Jugador.TirarObjeto.WasPressedThisFrame())
            {
                IntentarLanzarObjeto();
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

        objetoEquipadoActual = Instantiate(nuevoItem.prefabMundo, socketManoDerecha);
        objetoEquipadoActual.transform.localPosition = nuevoItem.offsetPosicion;
        objetoEquipadoActual.transform.localEulerAngles = nuevoItem.offsetRotacion;

        ConfigurarObjetoComoMano(objetoEquipadoActual, true);
    }

    private void IntentarLanzarObjeto()
    {
        if (datosObjetoActual == null || datosObjetoActual.prefabMundo == null || inventoryManager == null || hotbarUI == null) return;

        estaLanzando = true;

        // 1. Ocultamos el objeto visualmente de la mano mientras se prepara el gesto
        if (objetoEquipadoActual != null)
        {
            objetoEquipadoActual.SetActive(false);
        }

        // 2. Activamos el Trigger en el Animator para que Paula empiece a mover el brazo
        if (animatorPaula != null)
        {
            animatorPaula.SetTrigger("Lanzar");
        }
        else
        {
            // Si por algún motivo no hay Animator asignado, lanzamos al instante por seguridad
            EjecutarDisparoFisico();
        }
    }

    // --- ESTE MÉTODO ES LLAMADO POR EL ANIMATION EVENT EN EL FRAME EXACTO ---
    public void EjecutarDisparoFisico()
    {
        if (datosObjetoActual == null || datosObjetoActual.prefabMundo == null)
        {
            estaLanzando = false;
            return;
        }

        // 1. Destruimos el objeto fantasma de la mano
        if (objetoEquipadoActual != null)
        {
            Destroy(objetoEquipadoActual);
        }

        // 2. Instanciamos el clon físico en la posición de la mano
        GameObject objetoLanzado = Instantiate(datosObjetoActual.prefabMundo, socketManoDerecha.position, socketManoDerecha.rotation);

        // 3. Activación física
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

        // 4. Disparar el objeto hacia adelante
        if (rb != null)
        {
            Vector3 direccionDisparo = camaraJugador != null ? camaraJugador.forward : transform.forward;
            rb.AddForce(direccionDisparo * fuerzaLanzamiento + Vector3.up * fuerzaHaciaArriba, ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)), ForceMode.Impulse);
        }

        // 5. Vaciar el slot de la Hotbar / Inventario
        int slotActivo = hotbarUI.ObtenerIndiceSlotActivo();
        inventoryManager.slots[slotActivo] = null;

        estaLanzando = false;
    }

    private void ConfigurarObjetoComomano(GameObject obj, bool enMano)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = enMano;

        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = !enMano;
        }

        MonoBehaviour[] scriptsMundo = obj.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scriptsMundo)
        {
            if (script is ObjetoRecogible || script is ObjetoInteractuable || script is ItemFisicoFrenado)
            {
                script.enabled = !enMano;
            }
        }
    }

    private void ConfigurarObjetoComoMano(GameObject obj, bool enMano)
    {
        ConfigurarObjetoComomano(obj, enMano);
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

    public ItemData ObtenerItemEquipado()
    {
        return datosObjetoActual;
    }
}