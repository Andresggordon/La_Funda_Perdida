using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventarioSlot : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int miIndice;
    [HideInInspector] public MenuInventario menuPadre;

    [Header("Referencias Visuales")]
    public Image iconoVisual; // Arrastra aquí el objeto hijo "Icono"

    public void OnPointerClick(PointerEventData eventData)
    {
        if (menuPadre != null)
        {
            menuPadre.ClickEnSlot(miIndice);
        }
    }

    public void ActualizarVisual(ItemData item)
    {
        if (iconoVisual == null) return;

        if (item != null && item.icono != null)
        {
            iconoVisual.sprite = item.icono;
            iconoVisual.enabled = true;
        }
        else
        {
            iconoVisual.sprite = null;
            iconoVisual.enabled = false;
        }
    }
}