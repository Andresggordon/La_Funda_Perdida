using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrophyManager : MonoBehaviour
{
    [Header("Persistencia")]
    public SaveManager saveManager;

    [Header("UI Textos")]
    public TextMeshProUGUI textNombre;
    public TextMeshProUGUI textDescripcion;

    [Header("Visualización 3D")]
    public Transform spawnPoint3D;
    public OrbitalViewer orbitalViewer;
    public TrophyGridManager gridManager;

    private GameObject currentModelInstance;

    private void OnEnable()
    {
        CargarYRefrescar();
    }

    public void CargarYRefrescar()
    {
        List<string> trofeosGuardados = new List<string>();

        // Leemos la "caja de mudanzas"
        if (saveManager != null && saveManager.ExistePartida())
        {
            DatosPartida datos = saveManager.CargarPartida();
            if (datos != null && datos.trofeosDesbloqueadosID != null)
            {
                trofeosGuardados = datos.trofeosDesbloqueadosID;
            }
        }

        // Le decimos a la Grid que pinte los botones
        if (gridManager != null)
        {
            gridManager.ConfigurarGaleria(trofeosGuardados);
        }
    }

    // Esta función recibe el trofeo directamente desde el botón pulsado
    public void MostrarTrofeo(TrofeoData data)
    {
        if (data == null) return;

        // Actualizamos los textos
        if (textNombre != null) textNombre.text = data.trophyName;
        if (textDescripcion != null) textDescripcion.text = data.description;

        // Borramos el modelo 3D anterior si lo hay
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
        }

        // Instanciamos el nuevo modelo 3D
        if (data.prefabGaleria3D != null && spawnPoint3D != null)
        {
            currentModelInstance = Instantiate(data.prefabGaleria3D, spawnPoint3D.position, spawnPoint3D.rotation, spawnPoint3D);
            if (orbitalViewer != null)
            {
                orbitalViewer.SetTarget(currentModelInstance.transform);
            }
        }
    }
}