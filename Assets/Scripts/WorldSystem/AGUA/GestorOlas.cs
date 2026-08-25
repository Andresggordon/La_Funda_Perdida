using UnityEngine;

public class GestorOlas : MonoBehaviour
{
    public static GestorOlas Instancia; // Singleton para fácil acceso desde los objetos flotantes

    [Header("Ajustes del Oleaje")]
    public float alturaBaseAgua = 0f;
    public float amplitudOla = 0.5f;
    public float longitudOla = 2f;
    public float velocidadOla = 1f;

    private void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    // Esta función devuelve la altura del agua en una posición exacta
    public float ObtenerAlturaAgua(Vector3 posicionObjeto)
    {
        // Usamos una onda senoidal basada en el tiempo y las coordenadas X/Z para simular olas
        float offsetOleaje = Mathf.Sin(posicionObjeto.x * longitudOla + Time.time * velocidadOla) * amplitudOla;
        offsetOleaje += Mathf.Cos(posicionObjeto.z * longitudOla + Time.time * velocidadOla) * amplitudOla;

        return alturaBaseAgua + offsetOleaje;
    }
}