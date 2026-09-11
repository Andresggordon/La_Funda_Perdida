using UnityEngine;

// ScriptableObject con todos los ajustes visuales del ciclo dia/noche.
// Crea uno con: clic derecho en Project > Create > La Funda Perdida > Time Cycle Profile
[CreateAssetMenu(fileName = "NewTimeCycle", menuName = "La Funda Perdida/Time Cycle Profile")]
public class TimeCycleProfile : ScriptableObject
{
    [Header("Skybox BOXOPHOBIC (shader Skybox/Cubemap Blend)")]
    [Tooltip("0 = Dia (Cubemap HDR), 1 = Noche (Cubemap Blend HDR)")]
    public AnimationCurve skyboxBlendCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [Tooltip("Exposicion del cielo segun la hora")]
    public AnimationCurve skyboxExposureCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    [GradientUsage(true)] public Gradient skyboxTintColor;

    [Header("Luces Direccionales")]
    [Tooltip("IMPORTANTE para que la noche se vea oscura: el color/curva debe llegar a 0 en t=0 y t=1 (medianoche) y subir solo cerca de t=0.5 (mediodia). Ver instrucciones.")]
    [GradientUsage(true)] public Gradient sunColor;
    public AnimationCurve sunIntensity = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [GradientUsage(true)] public Gradient moonColor;
    public AnimationCurve moonIntensity = AnimationCurve.Linear(0f, 0f, 1f, 0.15f);

    [Header("Entorno (Atmosfera)")]
    [Tooltip("IMPORTANTE para la oscuridad nocturna: el color en t=0 y t=1 debe ser casi negro (ej. RGB 0.02-0.05), no gris medio")]
    [GradientUsage(true)] public Gradient ambientColor;

    [Header("Niebla")]
    [Tooltip("Interruptor general: desmarcalo para apagar la niebla por completo")]
    public bool fogEnabled = true;
    [Tooltip("Linear es mas facil de controlar (distancias). Exponential/ExponentialSquared usan 'Fog Density', muy sensible.")]
    public FogMode fogMode = FogMode.Linear;
    [GradientUsage(true)] public Gradient fogColor;
    [Tooltip("Solo si Fog Mode = Linear. Distancia donde EMPIEZA a notarse la niebla")]
    public AnimationCurve fogStartDistance = AnimationCurve.Constant(0f, 1f, 60f);
    [Tooltip("Solo si Fog Mode = Linear. Distancia donde la niebla ya es opaca del todo")]
    public AnimationCurve fogEndDistance = AnimationCurve.Constant(0f, 1f, 400f);
    [Tooltip("Solo si Fog Mode = Exponential o Exponential Squared. Tipico: 0.0005 a 0.01. NUNCA 0.02 o mas.")]
    public AnimationCurve fogDensity = AnimationCurve.Constant(0f, 1f, 0.002f);
}