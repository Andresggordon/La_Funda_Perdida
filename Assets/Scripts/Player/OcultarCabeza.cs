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
            // Si estamos en 1ª persona Y el juego NO está pausado (inventario cerrado)
            if (scriptCamara.estadoCamara == estadoPrimeraPersona && Time.timeScale > 0f)
            {
                huesoCabeza.localScale = Vector3.zero;
            }
            else
            {
                // Si pasamos a 3ª persona O abrimos el inventario, devolvemos la cabeza
                huesoCabeza.localScale = tamañoOriginal;
            }
        }
    }
}