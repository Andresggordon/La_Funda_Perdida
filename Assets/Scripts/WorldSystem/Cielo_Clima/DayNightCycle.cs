using UnityEngine;

// Sistema de ciclo dia/noche pensado para usarse junto al asset
// "Skybox Cubemap Extended" (Boxophobic / Amplify Shader Editor).
//
// COMO USARLO:
// 1) Crea un GameObject vacio "DayNightManager" y agrega este script.
// 2) Crea dos Directional Light: "Sun" y "Moon", y arrastralos a los
//    campos Sun Light / Moon Light de este componente.
// 3) Arrastra el material de tu skybox (Environment > Skybox Material)
//    al campo Skybox Material.
// 4) Verifica en modo Debug del Inspector del material cuales son los
//    nombres reales de las propiedades del shader y ajustalos en los
//    campos "Skybox - Cubemap Extended" de abajo si difieren.
// 5) Ajusta las curvas/gradientes a tu gusto y dale Play.
[ExecuteAlways]
public class DayNightCycle : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Luz direccional que representa el Sol")]
    public Light sunLight;
    [Tooltip("Luz direccional que representa la Luna")]
    public Light moonLight;
    [Tooltip("Material del Skybox Cubemap Extended asignado en Lighting > Environment")]
    public Material skyboxMaterial;

    [Header("Tiempo")]
    [Range(0f, 24f)]
    [Tooltip("Hora actual del dia (0 = medianoche, 12 = mediodia)")]
    public float timeOfDay = 8f;
    [Tooltip("Duracion de un dia completo, en segundos de tiempo real")]
    public float dayDurationInSeconds = 300f;
    [Tooltip("Si esta activo, el tiempo avanza solo mientras el juego corre")]
    public bool playAutomatically = true;

    [Header("Orientacion de la orbita")]
    [Tooltip("Inclinacion del recorrido del sol/luna respecto al horizonte")]
    public float axisTilt = 20f;
    [Tooltip("Rotacion horizontal (hacia que punto cardinal sale el sol)")]
    public float axisYaw = 0f;

    [Header("Intensidad de luz")]
    public AnimationCurve sunIntensityCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public AnimationCurve moonIntensityCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float maxSunIntensity = 1.2f;
    public float maxMoonIntensity = 0.15f;

    [Header("Color")]
    [Tooltip("Color de la luz del sol a lo largo del dia (evaluado 0-1)")]
    public Gradient sunColor;
    [Tooltip("Color ambiental / tinte del skybox a lo largo del dia")]
    public Gradient ambientColor;
    [Tooltip("Color de la niebla de escena a lo largo del dia")]
    public Gradient fogColor;

    [Header("Skybox - Cubemap Extended (nombres de propiedad del shader)")]
    [Tooltip("Verifica el nombre real en el Inspector del material en modo Debug")]
    public string skyboxExposureProperty = "_Exposure";
    public string skyboxTintProperty = "_TintColor";
    public string skyboxRotationProperty = "_Rotation";
    public string skyboxFogIntensityProperty = "_FogIntensity";
    public AnimationCurve skyboxExposureCurve = AnimationCurve.EaseInOut(0f, 0.4f, 1f, 1.3f);
    public AnimationCurve skyboxFogIntensityCurve = AnimationCurve.Constant(0f, 1f, 0.5f);
    [Tooltip("Si esta activo, la rotacion visual del cubemap sigue la posicion del sol")]
    public bool syncSkyboxRotationWithSun = true;

    [Header("Niebla de escena (RenderSettings)")]
    public bool syncSceneFog = true;
    public AnimationCurve fogDensityCurve = AnimationCurve.Constant(0f, 1f, 0.02f);

    [Header("Actualizacion de reflejos (costoso, usar con moderacion)")]
    public bool updateEnvironmentReflections = false;
    public float reflectionUpdateInterval = 10f;

    float sunAngle;
    float reflectionTimer;

    float NormalizedTime => timeOfDay / 24f;

    void Update()
    {
        if (playAutomatically && Application.isPlaying)
        {
            timeOfDay += (Time.deltaTime / Mathf.Max(dayDurationInSeconds, 0.01f)) * 24f;
            if (timeOfDay >= 24f) timeOfDay -= 24f;
        }

        UpdateCelestialRotation();
        UpdateLights();
        UpdateSkybox();
        UpdateSceneFog();
        UpdateReflections();
    }

    void UpdateCelestialRotation()
    {
        // 0h -> luna en el cenit, 12h -> sol en el cenit
        sunAngle = NormalizedTime * 360f - 90f;

        Quaternion baseRotation = Quaternion.Euler(0f, axisYaw, axisTilt);

        if (sunLight != null)
            sunLight.transform.rotation = baseRotation * Quaternion.Euler(sunAngle, 0f, 0f);

        if (moonLight != null)
            moonLight.transform.rotation = baseRotation * Quaternion.Euler(sunAngle + 180f, 0f, 0f);
    }

    void UpdateLights()
    {
        float t = NormalizedTime;

        if (sunLight != null)
        {
            float sunI = sunIntensityCurve.Evaluate(t) * maxSunIntensity;
            sunLight.intensity = sunI;
            sunLight.color = sunColor.Evaluate(t);
            sunLight.enabled = sunI > 0.001f;
        }

        if (moonLight != null)
        {
            float moonI = moonIntensityCurve.Evaluate(t) * maxMoonIntensity;
            moonLight.intensity = moonI;
            moonLight.enabled = moonI > 0.001f;
        }

        // Define cual de las dos luces maneja sombras / RenderSettings.sun
        if (sunLight != null && moonLight != null)
        {
            RenderSettings.sun = (sunLight.intensity >= moonLight.intensity) ? sunLight : moonLight;
        }

        if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Flat ||
            RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Trilight)
        {
            RenderSettings.ambientLight = ambientColor.Evaluate(t);
        }
    }

    void UpdateSkybox()
    {
        if (skyboxMaterial == null) return;
        float t = NormalizedTime;

        if (!string.IsNullOrEmpty(skyboxExposureProperty) && skyboxMaterial.HasProperty(skyboxExposureProperty))
            skyboxMaterial.SetFloat(skyboxExposureProperty, skyboxExposureCurve.Evaluate(t));

        if (!string.IsNullOrEmpty(skyboxTintProperty) && skyboxMaterial.HasProperty(skyboxTintProperty))
            skyboxMaterial.SetColor(skyboxTintProperty, ambientColor.Evaluate(t));

        if (syncSkyboxRotationWithSun && !string.IsNullOrEmpty(skyboxRotationProperty) &&
            skyboxMaterial.HasProperty(skyboxRotationProperty))
        {
            skyboxMaterial.SetFloat(skyboxRotationProperty, sunAngle);
        }

        if (!string.IsNullOrEmpty(skyboxFogIntensityProperty) && skyboxMaterial.HasProperty(skyboxFogIntensityProperty))
            skyboxMaterial.SetFloat(skyboxFogIntensityProperty, skyboxFogIntensityCurve.Evaluate(t));
    }

    void UpdateSceneFog()
    {
        if (!syncSceneFog) return;
        float t = NormalizedTime;
        RenderSettings.fogColor = fogColor.Evaluate(t);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(t);
    }

    void UpdateReflections()
    {
        if (!updateEnvironmentReflections || !Application.isPlaying) return;
        reflectionTimer += Time.deltaTime;
        if (reflectionTimer >= reflectionUpdateInterval)
        {
            reflectionTimer = 0f;
            DynamicGI.UpdateEnvironment();
        }
    }
}