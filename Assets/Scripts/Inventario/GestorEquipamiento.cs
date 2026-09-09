using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestorEquipamiento : MonoBehaviour
{
    [Header("Referencias de Equipamiento")]
    public Transform socketManoDerecha;
    public Animator animatorPaula; 

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
        if (estaLanzando) return; // Bloqueo anti-spam

        if (InputManager.Instancia != null && InputManager.Instancia.controles != null)
        {
            if (InputManager.Instancia.controles.Jugador.TirarObjeto.WasPressedThisFrame())
            {
                IntentarLanzarObjeto();
            }

            if (SeHaPulsadoUsar())
            {
                IntentarUsarObjeto();
            }
        }
    }

    private static bool SeHaPulsadoUsar()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) return true;
        if (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame) return true;
        return false;
    }

    private void IntentarUsarObjeto()
    {
        if (Time.timeScale == 0f || datosObjetoActual == null || inventoryManager == null || hotbarUI == null) return;

        bool seConsumio = datosObjetoActual.Usar(gameObject);
        if (!seConsumio) return;

        int slotActivo = hotbarUI.ObtenerIndiceSlotActivo();
        if (slotActivo >= 0 && slotActivo < inventoryManager.slots.Length)
            inventoryManager.slots[slotActivo] = null;

        if (objetoEquipadoActual != null) Destroy(objetoEquipadoActual);

        objetoEquipadoActual = null;
        datosObjetoActual = null;
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

        // --- ARREGLO DE DEFORMACIÓN ---
        // El 'false' obliga a Unity a ignorar la escala mundial deformada del hueso de la mano
        objetoEquipadoActual = Instantiate(nuevoItem.prefabMundo, socketManoDerecha, false);
        
        // Forzamos reseteo absoluto antes de aplicar offsets
        objetoEquipadoActual.transform.localPosition = Vector3.zero;
        objetoEquipadoActual.transform.localRotation = Quaternion.identity;
        objetoEquipadoActual.transform.localScale = Vector3.one;

        // Ahora aplicamos tus ajustes finos del ScriptableObject
        objetoEquipadoActual.transform.localPosition = nuevoItem.offsetPosicion;
        objetoEquipadoActual.transform.localEulerAngles = nuevoItem.offsetRotacion;
        objetoEquipadoActual.transform.localScale = nuevoItem.offsetEscala; 

        ConfigurarObjetoEnMano(objetoEquipadoActual, true);
    }

    private void IntentarLanzarObjeto()
    {
        if (datosObjetoActual == null || datosObjetoActual.prefabMundo == null || inventoryManager == null || hotbarUI == null) return;

        estaLanzando = true;

        // Ocultamos el objeto visualmente de la mano mientras se prepara el gesto
        if (objetoEquipadoActual != null) objetoEquipadoActual.SetActive(false);

        if (animatorPaula != null)
        {
            // Disparamos la animación. El Animation Event llamará a EjecutarDisparoFisico() en el frame perfecto.
            animatorPaula.SetTrigger("Lanzar");
            
            // Mantenemos un Invoke MÁS LARGO solo como sistema de seguridad anti-bloqueos.
            // Si por algún motivo la animación falla y no lanza el evento, a los 1.5s forzamos el disparo.
            Invoke(nameof(EjecutarDisparoFisico), 1.5f); 
        }
        else
        {
            // Si no hay animator, disparamos al instante
            EjecutarDisparoFisico();
        }
    }

    public void EjecutarDisparoFisico()
    {
        CancelInvoke(nameof(EjecutarDisparoFisico));

        if (datosObjetoActual == null || datosObjetoActual.prefabMundo == null)
        {
            estaLanzando = false;
            return;
        }

        if (objetoEquipadoActual != null) Destroy(objetoEquipadoActual);

        // 1. Instanciamos el clon físico (Que YA TRAE sus propias físicas configuradas en su Prefab)
        GameObject objetoLanzado = Instantiate(datosObjetoActual.prefabMundo, socketManoDerecha.position, camaraJugador != null ? camaraJugador.rotation : socketManoDerecha.rotation);

        // 2. Solo "despertamos" el Rigidbody original, respetando su masa, drag y material
        Rigidbody rb = objetoLanzado.GetComponent<Rigidbody>();
        if (rb != null) 
        {
            rb.isKinematic = false;
            // Forzamos Continuous solo para evitar que atraviese paredes al salir disparado muy rápido
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; 
        }

        Collider[] colliders = objetoLanzado.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders) col.enabled = true;

        ItemFisicoFrenado frenado = objetoLanzado.GetComponent<ItemFisicoFrenado>();
        if (frenado != null) frenado.enabled = true;

        StartCoroutine(ActivarRecogidaConRetraso(objetoLanzado));

        Collider colJugador = GetComponent<Collider>();
        foreach (Collider colObjeto in colliders)
        {
            if (colJugador != null && !colObjeto.isTrigger)
            {
                Physics.IgnoreCollision(colObjeto, colJugador);
            }
        }

        // 3. Disparamos aplicando la fuerza
        if (rb != null)
        {
            Vector3 direccionDisparo = camaraJugador != null ? camaraJugador.forward : transform.forward;
            
            // Usamos ForceMode.Impulse. Como la masa ahora depende del Prefab, 
            // los objetos pesados volarán menos lejos que los ligeros (¡Física realista automática!)
            rb.AddForce(direccionDisparo * fuerzaLanzamiento + Vector3.up * fuerzaHaciaArriba, ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)), ForceMode.Impulse);
        }

        int slotActivo = hotbarUI.ObtenerIndiceSlotActivo();
        inventoryManager.slots[slotActivo] = null;
        
        datosObjetoActual = null; 
        estaLanzando = false; 
    }

    private void ConfigurarObjetoEnMano(GameObject obj, bool enMano)
    {
        // 1. BLINDAJE DE FÍSICAS: Buscamos Rigidbodies en la raíz Y en todos los hijos
        Rigidbody[] rbs = obj.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = enMano; // Si está en la mano, lo congelamos
            rb.useGravity = !enMano; // Apagamos la gravedad para que no tire del brazo
        }

        // 2. BLINDAJE DE COLISIONES: Apagamos todos los colliders
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = !enMano;
        }

        // 3. BLINDAJE DE SCRIPTS: Apagamos scripts que puedan mover el objeto
        MonoBehaviour[] scriptsMundo = obj.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scriptsMundo)
        {
            if (script is ObjetoRecogible || script is ObjetoInteractuable || script is ItemFisicoFrenado)
            {
                script.enabled = !enMano;
            }
        }
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