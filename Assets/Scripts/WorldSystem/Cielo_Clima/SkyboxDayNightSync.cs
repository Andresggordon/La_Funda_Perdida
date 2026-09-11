using UnityEngine;

// Sincroniza el material "Skybox/Cubemap Blend" (el que ya trae tu asset,
// con los campos Cubemap (HDR) / Cubemap Blend (HDR) / Cubemap Transition)
// con la posicion real del Sol y la Luna.
//
// COMO USARLO:
// 1) Arrastra tu Sun (Directional Light) al campo "Sun Light".
// 2) Arrastra tu material (el que tiene el shader Skybox/Cubemap Blend)
//    al campo "Sky Material".
// 3) Dale Play y mueve/rota el sol (a mano o con tu propio sistema de
//    tiempo) para comprobar que el slider Cubemap Transition del
//    material se mueve solo en el Inspector.
//
// Si el cielo no cambia, revisa el nombre del campo "Cubemap Transition
// Property" en modo Debug del material (clic derecho en la pestaña del
// Inspector > Debug) y ajustalo aqui si es distinto.
[ExecuteAlways]
public class SkyboxDayNightSync : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("La luz direccional que hace de Sol")]
    public Light sunLight;
    [Tooltip("El material con el shader Skybox/Cubemap Blend")]
    public Material skyMaterial;

    [Header("Nombre interno de la propiedad (verificar en modo Debug)")]
    public string cubemapTransitionProperty = "_CubemapTransition";

    [Header("Curva de transicion")]
    [Tooltip("Eje X: 0 = sol en el punto mas bajo (medianoche), 1 = sol en el punto mas alto (mediodia). Eje Y: valor que se envia a Cubemap Transition (0 = cubemap A / dia, 1 = cubemap B / noche)")]
    public AnimationCurve transitionCurve = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.45f, 1f),
        new Keyframe(0.55f, 0f),
        new Keyframe(1f, 0f)
    );

    void Update()
    {
        if (sunLight == null || skyMaterial == null) return;

        // "forward" de una Directional Light es la direccion en la que
        // viaja la luz. Cuando el sol esta en su punto mas alto, esa
        // direccion apunta casi recta hacia abajo.
        float sunHeight = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        // Lo normalizamos de [-1, 1] a [0, 1] para usarlo en la curva
        float sunHeight01 = (sunHeight + 1f) * 0.5f;

        float transitionValue = transitionCurve.Evaluate(sunHeight01);

        if (skyMaterial.HasProperty(cubemapTransitionProperty))
            skyMaterial.SetFloat(cubemapTransitionProperty, transitionValue);
    }
}