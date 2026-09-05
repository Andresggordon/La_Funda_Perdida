using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrophySlotUI : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [Header("Identidad del Hueco")]
    [Tooltip("Arrastra aquí el ScriptableObject de este trofeo específico.")]
    public TrofeoData trofeoAsignado;

    [Header("Referencias UI")]
    public Image iconImage;
    public Button slotButton;

    private bool isUnlocked;
    private TrophyGridManager myGridManager;

    // El Manager llamará a esto para decirle al botón si está bloqueado o no
    public void Inicializar(bool desbloqueado, TrophyGridManager manager)
    {
        myGridManager = manager;
        isUnlocked = desbloqueado;

        // Si no le has asignado un trofeo en el Inspector, evitamos errores
        if (trofeoAsignado == null) return;

        // Ponemos la imagen del trofeo
        iconImage.sprite = trofeoAsignado.icono2D;

        if (isUnlocked)
        {
            // Desbloqueado: Color real, botón interactuable
            iconImage.color = Color.white;
            slotButton.interactable = true;
        }
        else
        {
            // Bloqueado: Imagen tintada de negro puro, no se puede hacer clic
            iconImage.color = Color.black; 
            slotButton.interactable = false;
        }

        // Configuramos el evento de clic
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(NotificarSeleccion);
    }

    public void OnSelect(BaseEventData eventData)
    {
        NotificarSeleccion();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isUnlocked)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }

    private void NotificarSeleccion()
    {
        if (isUnlocked && myGridManager != null && trofeoAsignado != null)
        {
            // En vez de pasar solo el ID, pasamos toda la data del trofeo
            myGridManager.OnSlotSelected(trofeoAsignado);
        }
    }
}