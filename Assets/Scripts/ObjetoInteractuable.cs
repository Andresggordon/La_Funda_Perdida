using UnityEngine;

// Obligamos a que el objeto tenga un Renderer (para poder pintarlo), evitando errores
[RequireComponent(typeof(Renderer))]
public class ObjetoInteractuable : MonoBehaviour
{
    private Renderer renderizador;
    private Color colorOriginal;

    private void Awake()
    {
        // Cacheamos las referencias al inicio (Buenas prácticas)
        renderizador = GetComponent<Renderer>();
        // Guardamos su color original para poder devolvérselo luego
        colorOriginal = renderizador.material.color;
    }

    // Este método lo llamará el jugador cuando le apunte con la cámara
    public void ActivarResaltado()
    {
        renderizador.material.color = Color.yellow;
    }

    // Este método lo llamará el jugador cuando mire hacia otro lado
    public void DesactivarResaltado()
    {
        renderizador.material.color = colorOriginal;
    }
}