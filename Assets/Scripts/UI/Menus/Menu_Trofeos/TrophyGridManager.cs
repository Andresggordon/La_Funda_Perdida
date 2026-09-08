using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrophyGridManager : MonoBehaviour
{
    [Header("Gestor Principal")]
    public TrophyManager trophyManager;

    [Header("Contenedor de los Slots")]
    public Transform gridContainer;

    public void ConfigurarGaleria(List<string> trofeosDesbloqueados)
    {
        TrophySlotUI[] slotsEnEscena = gridContainer.GetComponentsInChildren<TrophySlotUI>();
        GameObject primerBotonDesbloqueado = null;

        foreach (TrophySlotUI slot in slotsEnEscena)
        {
            if (slot.trofeoAsignado == null) continue;

            bool desbloqueado = trofeosDesbloqueados != null && trofeosDesbloqueados.Contains(slot.trofeoAsignado.trophyID);
            slot.Inicializar(desbloqueado, this);

            // AUTO-SELECCIÓN PARA MANDO
            if (desbloqueado && primerBotonDesbloqueado == null)
            {
                primerBotonDesbloqueado = slot.gameObject;
                
                // ¡AQUÍ ESTABA EL ERROR! Faltaba añadir ', desbloqueado'
                OnSlotSelected(slot.trofeoAsignado, desbloqueado); 
            }
        }

        // Si encontramos un botón, forzamos el foco del EventSystem
        if (primerBotonDesbloqueado != null)
        {
            EventSystem.current.SetSelectedGameObject(primerBotonDesbloqueado);
        }
    }
public void OnSlotSelected(TrofeoData trofeo, bool isUnlocked)
    {
        if (trophyManager != null)
        {
            trophyManager.MostrarTrofeoEnInspeccion(trofeo, isUnlocked);
        }
    }
}