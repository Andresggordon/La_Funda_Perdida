using UnityEngine;

// Aplica el TimeCycleProfile (curvas/gradientes) a las luces, al
// material del skybox (shader Skybox/Cubemap Blend) y a la niebla de
// escena, leyendo la hora desde GestorTiempoMundo.Instancia.
//
// Este script REEMPLAZA a los antiguos DayNightCycle.cs y
// SkyboxDayNightSync.cs. Esos dos ya no deben existir como
// componentes en ninguna escena (puedes borrar los archivos).
[ExecuteAlways]
public class SkyLightingController : MonoBehaviour
{
    [Header("Referencias")]
    public Light sunLight;
    public Light moonLight;
    [Tooltip("El material que usa el shader Skybox/Cubemap Blend")]
    public Material skyMaterial;
    [Tooltip("Asset ScriptableObject con las curvas/gradientes del ciclo")]
    public TimeCycleProfile profile;

    [Header("Nombres internos del shader (verificar en modo Debug si algo no se mueve)")]
    [Tooltip("Confirmado que funciona: _CubemapTransition")]
    public string cubemapTransitionProperty = "_CubemapTransition";
    public string cubemapExposureProperty = "_CubemapExposure";
    public string cubemapTintProperty = "_CubemapTintColor";

    [Header("Niebla de escena")]
    public bool controlSceneFog = true;

    void LateUpdate()
    {
        if (profile == null || GestorTiempoMundo.Instancia == null) return;

        float t = GestorTiempoMundo.Instancia.horaActual / 24f;

        ActualizarLuces(t);
        ActualizarSkybox(t);
        ActualizarAmbienteYNiebla(t);
    }

    void ActualizarLuces(float t)
    {
        if (sunLight != null)
        {
            sunLight.intensity = profile.sunIntensity.Evaluate(t);
            sunLight.color = profile.sunColor.Evaluate(t);
            sunLight.enabled = sunLight.intensity > 0.001f;
        }

        if (moonLight != null)
        {
            moonLight.intensity = profile.moonIntensity.Evaluate(t);
            moonLight.color = profile.moonColor.Evaluate(t);
            moonLight.enabled = moonLight.intensity > 0.001f;
        }

        // Cual de las dos luces maneja sombras / RenderSettings.sun
        if (sunLight != null && moonLight != null)
            RenderSettings.sun = (sunLight.intensity >= moonLight.intensity) ? sunLight : moonLight;
    }

    void ActualizarSkybox(float t)
    {
        if (skyMaterial == null) return;

        if (skyMaterial.HasProperty(cubemapTransitionProperty))
            skyMaterial.SetFloat(cubemapTransitionProperty, profile.skyboxBlendCurve.Evaluate(t));

        if (skyMaterial.HasProperty(cubemapExposureProperty))
            skyMaterial.SetFloat(cubemapExposureProperty, profile.skyboxExposureCurve.Evaluate(t));

        if (skyMaterial.HasProperty(cubemapTintProperty))
            skyMaterial.SetColor(cubemapTintProperty, profile.skyboxTintColor.Evaluate(t));
    }

    void ActualizarAmbienteYNiebla(float t)
    {
        Color ambiente = profile.ambientColor.Evaluate(t);
        RenderSettings.ambientSkyColor = ambiente;
        RenderSettings.ambientEquatorColor = ambiente;

        if (controlSceneFog)
        {
            RenderSettings.fogColor = profile.fogColor.Evaluate(t);
            RenderSettings.fogDensity = profile.fogDensity.Evaluate(t);
        }
    }
}