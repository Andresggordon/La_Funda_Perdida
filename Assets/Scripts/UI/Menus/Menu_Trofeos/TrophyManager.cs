using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrophyManager : MonoBehaviour
{
    [Header("Persistencia")]
    public SaveManager saveManager;

    [Header("Control de Paneles (UI)")]
    [Tooltip("El GameObject o Panel que contiene la parrilla de slots (Galería)")]
    public GameObject objetoPanelGaleria;     
    [Tooltip("El GameObject o Panel de la pantalla individual de inspección 3D")]
    public GameObject objetoPanelInspeccion; 

    [Header("UI Textos (Panel Derecho)")]
    public TextMeshProUGUI textNombre;
    public TextMeshProUGUI textDescripcion;

    [Header("Visualización 3D (Menú Principal)")]
    [Tooltip("El GameObject vacío colocado en la escena del menú para posicionar el 3D en la inspección")]
    public Transform spawnPointInspeccion3D; 
    public OrbitalViewer orbitalViewer;
    public TrophyGridManager gridManager;
    
    public Material materialSiluetaNegra; 

    private GameObject currentModelInstance;

    private void OnEnable()
    {
        RegresarAGaleria();
        CargarYRefrescar();
    }

    public void CargarYRefrescar()
    {
        List<string> trofeosGuardados = new List<string>();

        if (saveManager != null && saveManager.ExistePartida())
        {
            DatosPartida datos = saveManager.CargarPartida();
            if (datos != null && datos.trofeosDesbloqueadosID != null)
            {
                trofeosGuardados = datos.trofeosDesbloqueadosID;
            }
        }

        if (gridManager != null)
        {
            gridManager.ConfigurarGaleria(trofeosGuardados);
        }
    }

    // Se ejecuta al hacer clic en cualquier slot de la galería
    public void MostrarTrofeoEnInspeccion(TrofeoData data, bool isUnlocked)
    {
        if (data == null) return;

        // 1. Cambiamos de pantalla oculta/visible
        if (objetoPanelGaleria != null) objetoPanelGaleria.SetActive(false);
        if (objetoPanelInspeccion != null) objetoPanelInspeccion.SetActive(true);

        // 2. Limpiamos instancia previa si existía
        if (currentModelInstance != null) Destroy(currentModelInstance);

        // 3. Instanciamos el modelo 3D en el spawn point de la escena del menú
        if (data.prefabGaleria3D != null && spawnPointInspeccion3D != null)
        {
            currentModelInstance = Instantiate(data.prefabGaleria3D, spawnPointInspeccion3D.position, spawnPointInspeccion3D.rotation, spawnPointInspeccion3D);
            
            if (orbitalViewer != null)
            {
                orbitalViewer.SetTarget(currentModelInstance.transform);
            }

            // 4. Gestionamos la información textual y el material (silueta negra si está bloqueado)
            if (isUnlocked)
            {
                if (textNombre != null) textNombre.text = data.trophyName;
                if (textDescripcion != null) textDescripcion.text = data.description;
            }
            else
            {
                if (textNombre != null) textNombre.text = "???";
                if (textDescripcion != null) textDescripcion.text = "Aún no has encontrado este objeto en el mundo.";

                if (materialSiluetaNegra != null)
                {
                    Renderer[] renderizadores = currentModelInstance.GetComponentsInChildren<Renderer>();
                    foreach (Renderer render in renderizadores)
                    {
                        Material[] mats = new Material[render.materials.Length];
                        for (int i = 0; i < mats.Length; i++) mats[i] = materialSiluetaNegra;
                        render.materials = mats;
                    }
                }
            }
        }
    }

    // Conecta esto al botón "Volver" de la pantalla de inspección
    public void RegresarAGaleria()
    {
        if (currentModelInstance != null) Destroy(currentModelInstance);

        if (objetoPanelInspeccion != null) objetoPanelInspeccion.SetActive(false);
        if (objetoPanelGaleria != null) objetoPanelGaleria.SetActive(true);
    }
}