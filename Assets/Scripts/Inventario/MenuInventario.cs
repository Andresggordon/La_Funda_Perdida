using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Necesario para la posición segura del ratón

public class MenuInventario : MonoBehaviour
{
    [Header("Sistemas")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("Mochila")]
    [SerializeField] private GameObject panelInventarioUI;
    [SerializeField] private Transform contenedorSlots;
    [SerializeField] private GameObject prefabSlotInventario;

    [Header("Hotbar")]
    [SerializeField] private InventarioSlot[] slotsDelHotbar;

    [Header("Cursor de Arrastre (Estilo Minecraft)")]
    public Image iconoObjetoEnMano;

    private InventarioSlot[] todosLosSlotsVisuales;
    private bool inventarioAbierto = false;

    // Sistema estilo Minecraft: Objeto real en el cursor
    private ItemData itemEnMano = null;

    [Header("Vista 3D Personaje")]
    public GameObject camaraInventario3D; // Arrastra aquí la cámara que acabamos de crear
    public ControladorMiradaInventario controladorMirada; // Arrastra aquí el modelo de Paula


    private void Start()
    {
        if (panelInventarioUI != null) panelInventarioUI.SetActive(false);
        if (iconoObjetoEnMano != null)
        {
            iconoObjetoEnMano.gameObject.SetActive(false);

            // Evitamos que la imagen del cursor bloquee los rayos de UI
            CanvasGroup canvasGroup = iconoObjetoEnMano.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = iconoObjetoEnMano.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
        }

        ConstruirArquitecturaVisual();
    }

    private void ConstruirArquitecturaVisual()
    {
        if (inventoryManager == null) return;

        todosLosSlotsVisuales = new InventarioSlot[inventoryManager.capacidadTotal];

        // 1. Vincular Hotbar (Índices 0 al 3)
        for (int i = 0; i < 4; i++)
        {
            if (i < slotsDelHotbar.Length && slotsDelHotbar[i] != null)
            {
                slotsDelHotbar[i].miIndice = i;
                slotsDelHotbar[i].menuPadre = this;
                todosLosSlotsVisuales[i] = slotsDelHotbar[i];
            }
        }

        // 2. Instanciar Mochila (Índices 4 en adelante)
        if (contenedorSlots != null && prefabSlotInventario != null)
        {
            for (int i = 4; i < inventoryManager.capacidadTotal; i++)
            {
                GameObject nuevoObjeto = Instantiate(prefabSlotInventario, contenedorSlots);
                InventarioSlot scriptSlot = nuevoObjeto.GetComponent<InventarioSlot>();

                if (scriptSlot != null)
                {
                    scriptSlot.miIndice = i;
                    scriptSlot.menuPadre = this;
                    todosLosSlotsVisuales[i] = scriptSlot;
                }
            }
        }
    }

    private void Update()
    {
        // Validación de seguridad para evitar errores al iniciar el juego
        if (InputManager.Instancia == null || InputManager.Instancia.controles == null) return;

        // Si el tiempo está detenido por otro menú (como la pausa) y el inventario no está abierto, ignoramos
        if (Time.timeScale == 0f && !inventarioAbierto) return;

        var controles = InputManager.Instancia.controles;

        // 1. Alternar (Abrir/Cerrar) con la tecla principal de inventario (Ej: 'I' / Botón Options)
        if (controles.Jugador.AbrirInventario.WasPressedThisFrame())
        {
            if (inventarioAbierto) CerrarInventario();
            else AbrirInventario();
        }

        // 2. Cerrar con la tecla de cancelar (Ej: 'Esc' / Círculo en PS4) SOLO si está abierto
        if (inventarioAbierto && controles.UI.Cancel.WasPressedThisFrame())
        {
            CerrarInventario();
        }

        // Si el inventario está abierto, el icono en mano sigue al cursor del ratón de forma segura
        if (inventarioAbierto)
        {
            if (itemEnMano != null && iconoObjetoEnMano != null)
            {
                if (Mouse.current != null)
                {
                    iconoObjetoEnMano.transform.position = Mouse.current.position.ReadValue();
                }
            }
        }
    }

    // --- LÓGICA DE INTERCAMBIO ESTILO MINECRAFT ---
    public void ClickEnSlot(int indiceClick)
    {
        if (inventoryManager == null || indiceClick < 0 || indiceClick >= inventoryManager.slots.Length) return;

        ItemData itemEnSlot = inventoryManager.slots[indiceClick];

        if (itemEnMano == null)
        {
            // A. Mano vacía y slot con objeto -> Recogemos el objeto
            if (itemEnSlot != null)
            {
                itemEnMano = itemEnSlot;
                inventoryManager.slots[indiceClick] = null;
                ActualizarGraficoCursor();
            }
        }
        else
        {
            // B. La mano lleva un objeto
            if (itemEnSlot == null)
            {
                // Slot vacío -> Soltamos el objeto de la mano
                inventoryManager.slots[indiceClick] = itemEnMano;
                itemEnMano = null;
                ActualizarGraficoCursor();
            }
            else
            {
                // Slot ocupado -> Intercambio (Swap) entre mano y slot
                ItemData temp = itemEnSlot;
                inventoryManager.slots[indiceClick] = itemEnMano;
                itemEnMano = temp;
                ActualizarGraficoCursor();
            }
        }

        RefrescarVisuales();
    }

    private void ActualizarGraficoCursor()
    {
        if (iconoObjetoEnMano == null) return;

        if (itemEnMano != null && itemEnMano.icono != null)
        {
            iconoObjetoEnMano.sprite = itemEnMano.icono;
            iconoObjetoEnMano.gameObject.SetActive(true);
            iconoObjetoEnMano.transform.SetAsLastSibling();
        }
        else
        {
            iconoObjetoEnMano.sprite = null;
            iconoObjetoEnMano.gameObject.SetActive(false);
        }
    }

    private void AbrirInventario()
    {
        inventarioAbierto = true;
        panelInventarioUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // --- NUEVO: Activamos la vista 3D ---
        if (camaraInventario3D != null) camaraInventario3D.SetActive(true);
        if (controladorMirada != null) controladorMirada.rastrearRaton = true;

        if (iconoObjetoEnMano != null) iconoObjetoEnMano.transform.SetAsLastSibling();
        RefrescarVisuales();
    }

    private void CerrarInventario()
    {
        if (itemEnMano != null)
        {
            inventoryManager.AnadirObjeto(itemEnMano);
            itemEnMano = null;
            ActualizarGraficoCursor();
        }

        inventarioAbierto = false;
        panelInventarioUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // --- NUEVO: Desactivamos la vista 3D ---
        if (camaraInventario3D != null) camaraInventario3D.SetActive(false);
        if (controladorMirada != null) controladorMirada.rastrearRaton = false;

        RefrescarVisuales();
    }

    public void RefrescarVisuales()
    {
        if (todosLosSlotsVisuales == null || inventoryManager == null) return;

        for (int i = 0; i < todosLosSlotsVisuales.Length; i++)
        {
            if (todosLosSlotsVisuales[i] != null)
            {
                todosLosSlotsVisuales[i].ActualizarVisual(inventoryManager.slots[i]);
            }
        }
    }
}