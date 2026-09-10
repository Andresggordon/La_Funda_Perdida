// ==========================================
// SCRIPT: GestorTiempoMundo.cs
// UBICACIÓN: Scripts/WorldSystem/
// ==========================================
using System;
using System.Collections;
using UnityEngine;

public class GestorTiempoMundo : MonoBehaviour
{
    public static GestorTiempoMundo Instancia { get; private set; }

    [Header("Referencias de Escena")]
    public Light luzSol;
    public Light luzLuna;
    public Transform pivoteCielo;

    [Header("Tiempo")]
    [Range(0, 24)] public float horaActual = 12f;
    public float multiplicadorTiempo = 60f;

    [Header("Control Visual Continuo")]
    [Tooltip("Tiñe la luz del entorno (Pradera, Agua, Niebla). Eje X: 0=Noche, 0.5=Día, 1=Noche")]
    public Gradient gradienteColorEntorno;
    
    [Tooltip("Mezcla de texturas del Asset. 0 = Cubemap Día, 1 = Cubemap Noche")]
    public AnimationCurve curvaMezclaCielos = AnimationCurve.Linear(0, 1f, 1, 1f);

    [Tooltip("Exposición del cielo")]
    public AnimationCurve curvaExposicionCubemap = AnimationCurve.Linear(0, 0.2f, 1, 0.2f);
    
    [Tooltip("Intensidad de la Directional Light (Sol)")]
    public AnimationCurve curvaIntensidadSol = AnimationCurve.Linear(0, 0f, 1, 0f);

    private void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        
        horaActual += Time.deltaTime * (multiplicadorTiempo / 3600f) * 24f;
        if (horaActual >= 24f) horaActual = 0f;

        ActualizarRotacion();
        ActualizarIluminacionYAtmosfera();
    }

    private void ActualizarRotacion()
    {
        // El sol y la luna mantienen su rotación ininterrumpida
        float angulo = (horaActual - 6f) / 24f * 360f;
        if (luzSol != null) luzSol.transform.rotation = Quaternion.Euler(angulo, -30f, 0f);
        if (luzLuna != null) luzLuna.transform.rotation = Quaternion.Euler(angulo + 180f, -30f, 0f);
        if (pivoteCielo != null) pivoteCielo.rotation = Quaternion.Euler(angulo, 0f, 0f);
    }

    private void ActualizarIluminacionYAtmosfera()
    {
        float tiempoNormalizado = horaActual / 24f;

        // 1. Evaluamos Curvas y Gradientes
        Color colorEntorno = gradienteColorEntorno.Evaluate(tiempoNormalizado);
        float mezclaCielos = curvaMezclaCielos.Evaluate(tiempoNormalizado);
        float exposureActual = curvaExposicionCubemap.Evaluate(tiempoNormalizado);
        
        // 2. Aplicamos la iluminación al entorno (afecta a los props de la isla y la cueva del elefante)
        RenderSettings.ambientSkyColor = colorEntorno;
        RenderSettings.ambientEquatorColor = colorEntorno;
        RenderSettings.fogColor = colorEntorno; 

        // 3. Controlamos el Shader "Blend" de Boxophobic
        if (RenderSettings.skybox != null)
        {
            // _CubemapBlend es la variable interna del shader para transicionar entre texturas
            RenderSettings.skybox.SetFloat("_CubemapBlend", mezclaCielos); 
            RenderSettings.skybox.SetFloat("_Exposure", exposureActual);
            // Seguimos aplicando un ligero tinte para que la textura del cielo absorba los tonos del atardecer
            RenderSettings.skybox.SetColor("_TintColor", colorEntorno); 
        }

        // 4. Luces Direccionales
        if (luzSol != null) luzSol.intensity = curvaIntensidadSol.Evaluate(tiempoNormalizado);
        if (luzLuna != null) luzLuna.intensity = Mathf.Clamp01(0.18f - (luzSol.intensity * 0.5f));
    }

    public void DormirHastaHora(float horaDestino)
    {
        StartCoroutine(TransicionDormir(horaDestino));
    }

    private IEnumerator TransicionDormir(float horaDestino)
    {
        float tiempo = 2.5f;
        float elapsed = 0f;
        float inicio = horaActual;
        float objetivo = horaDestino < inicio ? horaDestino + 24f : horaDestino;

        while (elapsed < tiempo)
        {
            elapsed += Time.deltaTime;
            float progreso = Mathf.SmoothStep(0f, 1f, elapsed / tiempo);
            horaActual = Mathf.Lerp(inicio, objetivo, progreso) % 24f;
            ActualizarRotacion();
            ActualizarIluminacionYAtmosfera();
            yield return null;
        }
        horaActual = horaDestino;
        ActualizarRotacion();
        ActualizarIluminacionYAtmosfera();
    }
}