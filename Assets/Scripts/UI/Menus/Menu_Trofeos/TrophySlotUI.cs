using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrophySlotUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Referencias UI")]
    public Image iconImage;
    public Button slotButton;

    [Header("Configuración del Trofeo")]
    [Tooltip("Arrastra aquí el ScriptableObject TrofeoData correspondiente a este slot")]
    public TrofeoData trofeoAsignado;

    [HideInInspector] public bool isUnlocked;

    private TrophyGridManager myGridManager;

    public void Inicializar(bool desbloqueado, TrophyGridManager manager)
    {
        myGridManager = manager;
        isUnlocked = desbloqueado;

        if (slotButton == null) slotButton = GetComponent<Button>();

        if (slotButton != null)
        {
            slotButton.interactable = true;
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(NotificarSeleccion);
        }
    }

    private void NotificarSeleccion()
    {
        // Se ejecuta únicamente al hacer clic explícito o pulsar el botón de acción
        if (myGridManager != null && trofeoAsignado != null)
        {
            myGridManager.OnSlotSelected(trofeoAsignado, isUnlocked);
        }
    }

    // Foco visual del mando de PS4 (No abre pantallas por sí solo)
    public void OnSelect(BaseEventData eventData) { }
    public void OnDeselect(BaseEventData eventData) { }
}