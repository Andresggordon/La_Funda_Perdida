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

    [Header("Paleta Cromática Secuencial (Sin Grises ni Horizontes Amarillos)")]
    public Color colorMañanaAzul     = new Color(0.50f, 0.78f, 1.00f); // Azul limpio matutino
    public Color colorMediodiaNeutro = new Color(0.70f, 0.88f, 1.00f); // Azul claro diurno
    public Color colorAtardecerRosa  = new Color(0.95f, 0.45f, 0.70f); // Rosado suave
    public Color colorAtardecerClaro = new Color(1.00f, 0.65f, 0.35f); // Naranja claro
    public Color colorAtardecerFuerte= new Color(1.00f, 0.40f, 0.12f); // Naranja intenso
    public Color colorCrepusculo     = new Color(0.22f, 0.12f, 0.38f); // Morado crepuscular
    public Color colorNocheProfunda  = new Color(0.015f, 0.015f, 0.05f); // Noche profunda

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
        float angulo = (horaActual - 6f) / 24f * 360f;
        if (luzSol != null) luzSol.transform.rotation = Quaternion.Euler(angulo, -30f, 0f);
        if (luzLuna != null) luzLuna.transform.rotation = Quaternion.Euler(angulo + 180f, -30f, 0f);
        if (pivoteCielo != null) pivoteCielo.rotation = Quaternion.Euler(angulo, 0f, 0f);
    }

    private void ActualizarIluminacionYAtmosfera()
    {
        Color colorSkyTarget = colorMañanaAzul;
        float solIntensidad = 1.1f;
        float lunaIntensidad = 0f;
        float exposureVal = 1.2f;

        // 1. Mañana / Mediodía (06:00 a 16:00) -> Azul brillante y uniforme
        if (horaActual >= 6f && horaActual < 16f)
        {
            float t = (horaActual - 6f) / 10f;
            colorSkyTarget = Color.Lerp(colorMañanaAzul, colorMediodiaNeutro, t);
            solIntensidad = 1.1f;
            lunaIntensidad = 0f;
            exposureVal = 1.2f;
        }
        // 2. Tarde Inicial: Azul a Rosa (16:00 a 17:30)
        else if (horaActual >= 16f && horaActual < 17.5f)
        {
            float t = Mathf.SmoothStep(0f, 1f, (horaActual - 16f) / 1.5f);
            colorSkyTarget = Color.Lerp(colorMediodiaNeutro, colorAtardecerRosa, t);
            solIntensidad = Mathf.Lerp(1.1f, 0.95f, t);
            exposureVal = 1.25f;
        }
        // 3. Atardecer: Rosa a Naranja Claro (17:30 a 18:45)
        else if (horaActual >= 17.5f && horaActual < 18.75f)
        {
            float t = Mathf.SmoothStep(0f, 1f, (horaActual - 17.5f) / 1.25f);
            colorSkyTarget = Color.Lerp(colorAtardecerRosa, colorAtardecerClaro, t);
            solIntensidad = Mathf.Lerp(0.95f, 0.8f, t);
            exposureVal = 1.3f;
        }
        // 4. Ocaso: Naranja Claro a Naranja Intenso (18:75 a 20:00)
        else if (horaActual >= 18.75f && horaActual < 20f)
        {
            float t = Mathf.SmoothStep(0f, 1f, (horaActual - 18.75f) / 1.25f);
            colorSkyTarget = Color.Lerp(colorAtardecerClaro, colorAtardecerFuerte, t);
            solIntensidad = Mathf.Lerp(0.8f, 0.3f, t);
            exposureVal = 1.35f;
        }
        // 5. Crepúsculo: Naranja a Morado (20:00 a 21:30)
        else if (horaActual >= 20f && horaActual < 21.5f)
        {
            float t = Mathf.SmoothStep(0f, 1f, (horaActual - 20f) / 1.5f);
            colorSkyTarget = Color.Lerp(colorAtardecerFuerte, colorCrepusculo, t);
            solIntensidad = Mathf.Lerp(0.3f, 0f, t);
            lunaIntensidad = Mathf.Lerp(0f, 0.18f, t);
            exposureVal = Mathf.Lerp(1.35f, 0.5f, t);
        }
        // 6. Noche Profunda (21:30 a 05:00)
        else if (horaActual >= 21.5f || horaActual < 5f)
        {
            colorSkyTarget = colorNocheProfunda;
            solIntensidad = 0f;
            lunaIntensidad = 0.18f;
            exposureVal = 0.15f;
        }
        // 7. Amanecer Suavizado: Noche a Azul Mañana (05:00 a 06:00)
        else if (horaActual >= 5f && horaActual < 6f)
        {
            float t = Mathf.SmoothStep(0f, 1f, (horaActual - 5f) / 1f);
            colorSkyTarget = Color.Lerp(colorNocheProfunda, colorMañanaAzul, t);
            lunaIntensidad = Mathf.Lerp(0.18f, 0f, t);
            solIntensidad = Mathf.Lerp(0f, 1.1f, t);
            exposureVal = Mathf.Lerp(0.15f, 1.2f, t);
        }

        // Aplicación unificada para eliminar el horizonte amarillo y los grises
        RenderSettings.ambientSkyColor = colorSkyTarget;
        RenderSettings.ambientEquatorColor = colorSkyTarget;
        RenderSettings.fogColor = colorSkyTarget;

        if (RenderSettings.skybox != null)
        {
            // Forzamos tanto el cielo como el suelo al mismo color calculado, matando el corte
            RenderSettings.skybox.SetColor("_SkyTint", colorSkyTarget);
            RenderSettings.skybox.SetColor("_GroundColor", colorSkyTarget);
            RenderSettings.skybox.SetFloat("_Exposure", exposureVal);
            RenderSettings.skybox.SetFloat("_AtmosphereThickness", 0.5f); // Valor bajo que bloquea la dispersión amarilla
        }

        if (luzSol != null) luzSol.intensity = solIntensidad;
        if (luzLuna != null) luzLuna.intensity = lunaIntensidad;
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