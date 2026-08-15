using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private InventoryManager inventoryManager;

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
}