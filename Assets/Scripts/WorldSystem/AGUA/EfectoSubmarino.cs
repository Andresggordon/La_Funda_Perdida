using UnityEngine;

public class EfectoSubmarino : MonoBehaviour
{
    [Header("Referencias")]
    public Transform superficieAgua;

    [Header("Ajustes Visuales (Bajo el agua)")]
    public Color colorBajoAgua = new Color(0.1f, 0.4f, 0.7f, 1f);
    public float densidadBajoAgua = 0.02f;

    [Header("Ajustes Visuales (Fuera del agua)")]
    public Color colorFueraAgua = new Color(0.5f, 0.5f, 0.5f, 1f);
    public float densidadFueraAgua = 0.0f;

    [Header("Transición")]
    public float velocidadTransicion = 3f;

    private void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;

        // --- CORRECCIÓN DE INICIO ---
        // Comprobamos la posición de inmediato al arrancar el juego, 
        // sin esperar a que pasen los primeros fotogramas.
        if (superficieAgua != null)
        {
            if (transform.position.y < superficieAgua.position.y)
            {
                RenderSettings.fogColor = colorBajoAgua;
                RenderSettings.fogDensity = densidadBajoAgua;
            }
            else
            {
                RenderSettings.fogColor = colorFueraAgua;
                RenderSettings.fogDensity = densidadFueraAgua;
            }
        }
    }

    private void Update()
    {
        if (superficieAgua == null) return;

        // Comparamos la altura de la cámara con el agua
        if (transform.position.y < superficieAgua.position.y)
        {
            // Transición SUAVE al agua
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, colorBajoAgua, Time.deltaTime * velocidadTransicion);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, densidadBajoAgua, Time.deltaTime * velocidadTransicion);
        }
        else
        {
            // Transición SUAVE al aire
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, colorFueraAgua, Time.deltaTime * velocidadTransicion);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, densidadFueraAgua, Time.deltaTime * velocidadTransicion);
        }
    }
}