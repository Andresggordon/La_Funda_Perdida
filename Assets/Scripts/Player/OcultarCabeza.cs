using UnityEngine;

public class OcultarCabeza : MonoBehaviour
{
    [Header("Referencias")]
    public Transform huesoCabeza;
    public JugadorCamara scriptCamara; // Necesitamos leer tu estadoCamara

    [Header("Configuración")]
    [Tooltip("¿Qué número indica que estás en primera persona? (Normalmente 0 o 1)")]
    public int estadoPrimeraPersona = 0; 

    private Vector3 tamañoOriginal;

    void Start()
    {
        // Guardamos el tamaño normal de la cabeza al empezar el juego
        if (huesoCabeza != null)
        {
            tamañoOriginal = huesoCabeza.localScale;
        }
    }

    void LateUpdate()
    {
        if (huesoCabeza != null && scriptCamara != null)
        {
            // Si el estado coincide con la primera persona, encogemos
            if (scriptCamara.estadoCamara == estadoPrimeraPersona)
            {
                huesoCabeza.localScale = Vector3.zero;
            }
            // Si cambiamos a tercera persona, devolvemos su tamaño original
            else
            {
                huesoCabeza.localScale = tamañoOriginal;
            }
        }
    }
}