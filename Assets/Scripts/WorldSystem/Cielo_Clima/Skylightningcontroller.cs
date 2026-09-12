using UnityEngine;

// Aplica el TimeCycleProfile (curvas/gradientes) a las luces, al
// material del skybox (shader Skybox/Cubemap Blend) y a la niebla de
// escena, leyendo la hora desde GestorTiempoMundo.Instancia.
//
// Tambien anade un halo/brillo de atardecer que se activa segun la
// ALTURA REAL del sol (no la hora del reloj), asi que ocurre siempre
// justo cuando el sol esta bajo, sin importar la duracion del dia.
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
    public string cubemapTransitionProperty = "_CubemapTransition";
    public string cubemapExposureProperty = "_Exposure";
    public string cubemapTintProperty = "_TintColor";

    [Header("Resplandor (Bloom) de los quads del Sol y la Luna")]
    [Tooltip("El material Mat_Sol de tu quad Imagen_Sol")]
    public Material sunGlowMaterial;
    [Tooltip("El material Mat_Luna de tu quad Imagen_Luna")]
    public Material moonGlowMaterial;
    [Tooltip("Propiedad de color del shader Universal Render Pipeline/Unlit")]
    public string glowColorProperty = "_BaseColor";

    void LateUpdate()
    {
        if (profile == null || GestorTiempoMundo.Instancia == null) return;

        float t = GestorTiempoMundo.Instancia.horaActual / 24f;
        float sunElevation01 = ComputeSunElevation01();
        float glow = Mathf.Clamp01(profile.sunsetGlowCurve.Evaluate(sunElevation01) * profile.sunsetGlowIntensity);
        float exposureBoost = profile.sunsetExposureBoost.Evaluate(sunElevation01);

        ActualizarLuces(t, glow);
        ActualizarSkybox(t, glow, exposureBoost);
        ActualizarAmbiente(t);
        ActualizarNiebla(t);
        ActualizarBrilloAstros(t, glow);
    }

    // 0 = sol en el horizonte, 1 = sol en el cenit
    float ComputeSunElevation01()
    {
        if (sunLight == null) return 0.5f;
        float h = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        return Mathf.Clamp01((h + 1f) * 0.5f);
    }

    void ActualizarLuces(float t, float glow)
    {
        if (sunLight != null)
        {
            sunLight.intensity = profile.sunIntensity.Evaluate(t);
            Color baseColor = profile.sunColor.Evaluate(t);
            sunLight.color = Color.Lerp(baseColor, profile.sunsetGlowColor, glow);
            sunLight.enabled = sunLight.intensity > 0.001f;
        }

        if (moonLight != null)
        {
            moonLight.intensity = profile.moonIntensity.Evaluate(t);
            moonLight.color = profile.moonColor.Evaluate(t);
            moonLight.enabled = moonLight.intensity > 0.001f;
        }

        if (sunLight != null && moonLight != null)
            RenderSettings.sun = (sunLight.intensity >= moonLight.intensity) ? sunLight : moonLight;
    }

    void ActualizarSkybox(float t, float glow, float exposureBoost)
    {
        if (skyMaterial == null) return;

        if (skyMaterial.HasProperty(cubemapTransitionProperty))
            skyMaterial.SetFloat(cubemapTransitionProperty, profile.skyboxBlendCurve.Evaluate(t));

        if (skyMaterial.HasProperty(cubemapExposureProperty))
            skyMaterial.SetFloat(cubemapExposureProperty, profile.skyboxExposureCurve.Evaluate(t) + exposureBoost);

        if (skyMaterial.HasProperty(cubemapTintProperty))
        {
            Color baseTint = profile.skyboxTintColor.Evaluate(t);
            Color finalTint = Color.Lerp(baseTint, profile.sunsetGlowColor, glow);
            skyMaterial.SetColor(cubemapTintProperty, finalTint);
        }
    }

    void ActualizarAmbiente(float t)
    {
        // Forzamos el modo Trilight por codigo: si "Environment Lighting >
        // Source" esta en Skybox (el valor por defecto de Unity), nuestros
        // colores se ignorarian por completo sin este forzado.
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;

        Color ambiente = profile.ambientColor.Evaluate(t);
        RenderSettings.ambientSkyColor = ambiente;
        RenderSettings.ambientEquatorColor = ambiente;
        RenderSettings.ambientGroundColor = ambiente;
    }

    void ActualizarNiebla(float t)
    {
        RenderSettings.fog = profile.fogEnabled;
        if (!profile.fogEnabled) return;

        RenderSettings.fogMode = profile.fogMode;
        RenderSettings.fogColor = profile.fogColor.Evaluate(t);

        if (profile.fogMode == FogMode.Linear)
        {
            RenderSettings.fogStartDistance = profile.fogStartDistance.Evaluate(t);
            RenderSettings.fogEndDistance = profile.fogEndDistance.Evaluate(t);
        }
        else
        {
            RenderSettings.fogDensity = profile.fogDensity.Evaluate(t);
        }
    }

    // Empuja el color por encima de 1 (HDR) para que el Bloom del
    // Post Processing Volume lo detecte y le ponga resplandor.
    void ActualizarBrilloAstros(float t, float glow)
    {
        if (sunGlowMaterial != null && sunGlowMaterial.HasProperty(glowColorProperty))
        {
            Color warm = Color.Lerp(profile.sunColor.Evaluate(t), profile.sunsetGlowColor, glow);
            float boost = profile.sunIntensity.Evaluate(t) * profile.sunGlowMultiplier;
            sunGlowMaterial.SetColor(glowColorProperty, new Color(warm.r * boost, warm.g * boost, warm.b * boost, 1f));
        }

        if (moonGlowMaterial != null && moonGlowMaterial.HasProperty(glowColorProperty))
        {
            Color moonBase = profile.moonColor.Evaluate(t);
            float boost = profile.moonIntensity.Evaluate(t) * profile.moonGlowMultiplier;
            moonGlowMaterial.SetColor(glowColorProperty, new Color(moonBase.r * boost, moonBase.g * boost, moonBase.b * boost, 1f));
        }
    }
}