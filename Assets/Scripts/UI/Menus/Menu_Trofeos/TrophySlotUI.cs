using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con Image

public class TrophySlotUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Arrastra aquí el componente Image del icono del trofeo")]
    public Image iconoTrofeo;
    
    [Header("Datos (Se asignan solos o en el editor)")]
    public TrofeoData trofeoAsignado;
    
    private bool desbloqueado;
    private TrophyGridManager manager;

    public void Inicializar(bool estaDesbloqueado, TrophyGridManager gridManager)
    {
        desbloqueado = estaDesbloqueado;
        manager = gridManager;

        if (trofeoAsignado != null && iconoTrofeo != null)
        {
            // Asignamos la imagen que toca
            iconoTrofeo.sprite = trofeoAsignado.icono2D;
            iconoTrofeo.gameObject.SetActive(true);

            // CONTROL VISUAL: ¿Está desbloqueado?
            if (desbloqueado)
            {
                // A todo color
                iconoTrofeo.color = Color.white; 
            }
            else
            {
                // Silueta oscura y completamente opaca (Gris muy oscuro / Negro sólido con Alpha = 1f)
                iconoTrofeo.color = new Color(0.01f, 0.01f, 0.01f, 1f); 
            }
        }
        else if (iconoTrofeo != null)
        {
            iconoTrofeo.gameObject.SetActive(false);
        }
    }

    // Esta función debe estar conectada al evento OnClick() del botón en el Inspector
    public void OnClickSlot()
    {
        if (manager != null && trofeoAsignado != null)
        {
            manager.OnSlotSelected(trofeoAsignado, desbloqueado);
        }
    }
}