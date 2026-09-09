using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrophyManager : MonoBehaviour
{
    [Header("Persistencia")]
    public SaveManager saveManager;

    [Header("Control de Paneles (UI)")]
    public GameObject objetoPanelGaleria;     
    public GameObject objetoPanelInspeccion; 

    [Header("UI Textos (Panel Derecho)")]
    public TextMeshProUGUI textNombre;
    public TextMeshProUGUI textDescripcion;

    [Header("Visualización 3D (Menú Principal)")]
    public Transform spawnPointInspeccion3D; 
    public RotadorTrofeo3D rotadorTrofeo; 
    public TrophyGridManager gridManager;
    public Material materialSiluetaNegra; 

    private GameObject currentModelInstance;

    private void OnEnable()
    {
        if (objetoPanelGaleria != null) objetoPanelGaleria.SetActive(true);
        if (objetoPanelInspeccion != null) objetoPanelInspeccion.SetActive(false);
        if (currentModelInstance != null) Destroy(currentModelInstance);
        
        CargarYRefrescar();
    }

    // Solución al Bug del "Fantasma" en el menú principal
    private void OnDisable()
    {
        if (currentModelInstance != null) Destroy(currentModelInstance);
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

        if (gridManager != null) gridManager.ConfigurarGaleria(trofeosGuardados);
    }

    public void MostrarTrofeoEnInspeccion(TrofeoData data, bool isUnlocked)
    {
        if (data == null) return;

        if (objetoPanelGaleria != null) objetoPanelGaleria.SetActive(false);
        if (objetoPanelInspeccion != null) objetoPanelInspeccion.SetActive(true);

        if (currentModelInstance != null) Destroy(currentModelInstance);

        if (data.prefabGaleria3D != null && spawnPointInspeccion3D != null)
        {
            // Instanciación limpia y directa
            currentModelInstance = Instantiate(data.prefabGaleria3D, spawnPointInspeccion3D);
            
            Rigidbody rb = currentModelInstance.GetComponent<Rigidbody>();
            if (rb != null) Destroy(rb);

            currentModelInstance.transform.localPosition = Vector3.zero;
            currentModelInstance.transform.localEulerAngles = data.rotacionInicial;
            currentModelInstance.transform.localScale = data.escalaInspeccion;

            CambiarCapaRecursivo(currentModelInstance.transform, LayerMask.NameToLayer("UI"));

            if (rotadorTrofeo != null) rotadorTrofeo.SetTarget(currentModelInstance.transform);

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
                    // AQUÍ ESTABA EL FALLO: Usamos currentModelInstance en vez de modeloReal
                    Renderer[] renderizadores = currentModelInstance.GetComponentsInChildren<Renderer>(true);
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

    public void RegresarAGaleria()
    {
        if (currentModelInstance != null) Destroy(currentModelInstance);

        if (objetoPanelInspeccion != null) objetoPanelInspeccion.SetActive(false);
        if (objetoPanelGaleria != null) objetoPanelGaleria.SetActive(true);
    }

    private void CambiarCapaRecursivo(Transform objeto, int nuevaCapa)
    {
        objeto.gameObject.layer = nuevaCapa;
        foreach (Transform hijo in objeto)
        {
            CambiarCapaRecursivo(hijo, nuevaCapa);
        }
    }
}