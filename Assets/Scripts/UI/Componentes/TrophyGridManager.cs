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
        // 1. Buscamos TODOS los slots que has creado a mano dentro del GridContenedor
        TrophySlotUI[] slotsEnEscena = gridContainer.GetComponentsInChildren<TrophySlotUI>();
        
        GameObject primerBotonDesbloqueado = null;

        // 2. Revisamos uno por uno
        foreach (TrophySlotUI slot in slotsEnEscena)
        {
            if (slot.trofeoAsignado == null) continue;

            // 3. Comprobamos si el ID de este slot está en la lista de guardado del jugador
            bool desbloqueado = trofeosDesbloqueados != null && trofeosDesbloqueados.Contains(slot.trofeoAsignado.trophyID);

            // 4. Le decimos al slot que se configure (Negro o Color)
            slot.Inicializar(desbloqueado, this);

            // 5. Guardamos el primero que veamos desbloqueado para enfocar el Mando de PS4
            if (desbloqueado && primerBotonDesbloqueado == null)
            {
                primerBotonDesbloqueado = slot.gameObject;
                OnSlotSelected(slot.trofeoAsignado);
            }
        }

        // Enfocamos automáticamente el primer trofeo disponible para soporte de mando
        if (primerBotonDesbloqueado != null)
        {
            EventSystem.current.SetSelectedGameObject(primerBotonDesbloqueado);
        }
    }

    public void OnSlotSelected(TrofeoData trofeo)
    {
        if (trophyManager != null)
        {
            trophyManager.MostrarTrofeo(trofeo);
        }
    }
}