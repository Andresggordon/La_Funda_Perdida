using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorMiradaInventario : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("La cámara exclusiva del inventario que renderiza a Paula")]
    public Camera camaraInventario;

    [Tooltip("IMPORTANTE: Despliega el esqueleto de tu personaje y arrastra aquí el GameObject que corresponde al hueso del CUELLO o la CABEZA.")]
    public Transform huesoCabeza;

    [Header("Ajustes de Mirada")]
    public float suavidad = 15f;
    public float distanciaZVirtual = 2f; // Profundidad de proyección del ratón

    [HideInInspector] public bool rastrearRaton = false;

    private Vector3 posicionObjetivo;

    private void Update()
    {
        // Solo calculamos la posición si el inventario está abierto y tenemos las referencias
        if (!rastrearRaton || camaraInventario == null || Mouse.current == null || huesoCabeza == null) return;

        // Leemos la posición real del ratón usando el New Input System
        Vector2 ratonPos2D = Mouse.current.position.ReadValue();

        // Convertimos la posición de la pantalla a un punto en el espacio 3D real
        Vector3 ratonPos3D = new Vector3(ratonPos2D.x, ratonPos2D.y, distanciaZVirtual);
        posicionObjetivo = camaraInventario.ScreenToWorldPoint(ratonPos3D);
    }

    // Usamos LateUpdate para Rigs Generic. Se ejecuta justo DESPUÉS de que el Animator posicione los huesos.
    private void LateUpdate()
    {
        if (!rastrearRaton || huesoCabeza == null) return;

        // 1. Calculamos la dirección desde la cabeza hacia el cursor del ratón
        Vector3 direccionHaciaRaton = posicionObjetivo - huesoCabeza.position;

        // 2. Calculamos la rotación matemática necesaria para mirar en esa dirección
        if (direccionHaciaRaton != Vector3.zero)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccionHaciaRaton);

            // 3. Suavizamos la rotación de la cabeza (usando unscaledDeltaTime porque el juego está pausado a Time.timeScale = 0f)
            huesoCabeza.rotation = Quaternion.Slerp(huesoCabeza.rotation, rotacionDeseada, Time.unscaledDeltaTime * suavidad);
        }
    }
}