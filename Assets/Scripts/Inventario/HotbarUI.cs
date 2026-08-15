using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    [Header("Referencias Principales")]
    [SerializeField] private InventoryManager inventoryManager;

    [Tooltip("Arrastra aquí a Paula (el GameObject que tiene el script GestorEquipamiento)")]
    [SerializeField] private GestorEquipamiento gestorEquipamiento; // NUEVA REFERENCIA

    [Header("Slots Visuales de la Hotbar")]
    [Tooltip("Arrastra aquí los componentes 'InventarioSlot' de Slot_1, Slot_2, Slot_3 y Slot_4")]
    [SerializeField] private InventarioSlot[] slotsHotbarVisuales;

    [Header("Animación de Selección")]
    [SerializeField] private RectTransform[] contenedoresSlots;
    [SerializeField] private float escalaSeleccionado = 1.2f;
    [SerializeField] private float velocidadAnimacion = 15f;

    private int slotActivoIndex = 0;

    private void Update()
    {
        ComprobarInputsHotbar();
        SincronizarSlots();
        AnimarSeleccion();

        // Ejecutamos la nueva comprobación en tiempo real
        SincronizarModeloEnMano();
    }

    private void ComprobarInputsHotbar()
    {
        if (InputManager.Instancia.controles.Jugador.Slot1.WasPressedThisFrame()) slotActivoIndex = 0;
        if (InputManager.Instancia.controles.Jugador.Slot2.WasPressedThisFrame()) slotActivoIndex = 1;
        if (InputManager.Instancia.controles.Jugador.Slot3.WasPressedThisFrame()) slotActivoIndex = 2;
        if (InputManager.Instancia.controles.Jugador.Slot4.WasPressedThisFrame()) slotActivoIndex = 3;

        if (InputManager.Instancia.controles.Jugador.NavegarDerecha.WasPressedThisFrame())
        {
            slotActivoIndex++;
            if (slotActivoIndex >= contenedoresSlots.Length) slotActivoIndex = 0;
        }

        if (InputManager.Instancia.controles.Jugador.NavegarIzquierda.WasPressedThisFrame())
        {
            slotActivoIndex--;
            if (slotActivoIndex < 0) slotActivoIndex = contenedoresSlots.Length - 1;
        }
    }

    private void SincronizarSlots()
    {
        if (inventoryManager == null || slotsHotbarVisuales == null) return;

        // Los primeros 4 huecos (0 a 3) son la Hotbar
        for (int i = 0; i < slotsHotbarVisuales.Length; i++)
        {
            if (slotsHotbarVisuales[i] != null && i < inventoryManager.slots.Length)
            {
                slotsHotbarVisuales[i].miIndice = i;
                slotsHotbarVisuales[i].ActualizarVisual(inventoryManager.slots[i]);
            }
        }
    }

    private void AnimarSeleccion()
    {
        if (contenedoresSlots == null || contenedoresSlots.Length == 0) return;

        for (int i = 0; i < contenedoresSlots.Length; i++)
        {
            if (contenedoresSlots[i] == null) continue;

            Vector3 escalaDestino = (i == slotActivoIndex) ? Vector3.one * escalaSeleccionado : Vector3.one;
            contenedoresSlots[i].localScale = Vector3.Lerp(contenedoresSlots[i].localScale, escalaDestino, Time.unscaledDeltaTime * velocidadAnimacion);
        }
    }

    // --- LA MAGIA DEL RETO 1 ---
    private void SincronizarModeloEnMano()
    {
        // Seguridad por si aún no hemos enlazado las referencias en el Inspector
        if (gestorEquipamiento == null || inventoryManager == null) return;

        // 1. Miramos qué objeto hay en los datos internos de ese hueco de la mochila
        ItemData itemEnElSlotActivo = inventoryManager.slots[slotActivoIndex];

        // 2. Le preguntamos a Paula qué lleva físicamente en la mano ahora mismo
        ItemData itemFisicoEnMano = gestorEquipamiento.ObtenerItemEquipado();

        // 3. Si son diferentes (ej. pulsaste el '2', y había otro objeto, o lo acabas de tirar), actualizamos
        if (itemEnElSlotActivo != itemFisicoEnMano)
        {
            gestorEquipamiento.EquiparObjeto(itemEnElSlotActivo);
        }
    }

    // Función pública vital para que los Retos 2 y 3 puedan saber de dónde sacar el objeto
    public int ObtenerIndiceSlotActivo()
    {
        return slotActivoIndex;
    }
}