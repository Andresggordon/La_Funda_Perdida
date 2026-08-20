using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(100)]
public class ControladorMiradaInventario : MonoBehaviour
{
    [Header("Referencias Principales")]
    [Tooltip("El hueso Head o Neck dentro del esqueleto de Paula")]
    public Transform huesoCabeza;

    [Tooltip("Arrastra aquí el 'VistaPersonaje_RawImage' de tu UI. Así calculamos el centro exacto de la mirada.")]
    public RectTransform rawImagePersonaje;

    [Header("Límites de Mirada")]
    public float suavidad = 15f;
    public float anguloMaximoHorizontal = 45f;
    public float anguloMaximoVertical = 30f;

    [Header("Mapeo de Ejes (¡Solución al ladeo!)")]
    [Tooltip("Pon un 1 en el eje que hace que la cabeza diga 'NO'. Si se ladea, ponlo en 0 y prueba con el eje Z (0, 0, 1) o X (1, 0, 0).")]
    public Vector3 ejeHorizontal = new Vector3(0, 1, 0);

    [Tooltip("Pon un 1 en el eje que hace que la cabeza diga 'SÍ'.")]
    public Vector3 ejeVertical = new Vector3(1, 0, 0);

    [Header("Corrección Base")]
    [Tooltip("Úsalo si la cabeza mira hacia atrás o está rotada por defecto (Ej: 0, 180, 0)")]
    public Vector3 offsetRotacion;

    [Header("Auditoría")]
    public bool rastrearRaton = false;

    private float smoothX;
    private float smoothY;
    private Quaternion rotacionAnimacion;

    private void LateUpdate()
    {
        if (huesoCabeza == null || !rastrearRaton || Mouse.current == null) return;

        Vector2 ratonPos = Mouse.current.position.ReadValue();
        float normalX = 0f;
        float normalY = 0f;

        // 1. Calculamos dónde está el ratón pero SOLO relativo al recuadro de Paula
        if (rawImagePersonaje != null)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImagePersonaje, ratonPos, null, out Vector2 localPoint))
            {
                Rect rect = rawImagePersonaje.rect;
                // Normalizamos de -1 a 1 basado en el ancho y alto del RawImage
                normalX = Mathf.Clamp(localPoint.x / (rect.width / 2f), -1f, 1f);
                normalY = Mathf.Clamp(localPoint.y / (rect.height / 2f), -1f, 1f);
            }
        }

        // 2. Interpolar suavemente el movimiento 
        smoothX = Mathf.Lerp(smoothX, normalX, Time.unscaledDeltaTime * suavidad);
        smoothY = Mathf.Lerp(smoothY, normalY, Time.unscaledDeltaTime * suavidad);

        // 3. Generamos los grados a girar
        float gradosHorizontal = smoothX * anguloMaximoHorizontal;
        float gradosVertical = -smoothY * anguloMaximoVertical;

        // Guardamos cómo la dejó el Animator (por si está respirando o saltando)
        rotacionAnimacion = huesoCabeza.localRotation;

        // 4. LA MAGIA: Multiplicamos los grados por los ejes que elijas en el Inspector
        Vector3 rotacionDeseada = (ejeHorizontal * gradosHorizontal) + (ejeVertical * gradosVertical) + offsetRotacion;

        Quaternion rotacionMirada = Quaternion.Euler(rotacionDeseada);

        // 5. Aplicamos la rotación combinada
        huesoCabeza.localRotation = rotacionAnimacion * rotacionMirada;
    }
}