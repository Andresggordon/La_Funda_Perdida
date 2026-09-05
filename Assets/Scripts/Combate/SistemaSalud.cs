// ==========================================
// SCRIPT: SistemaSalud.cs
// ==========================================
using System;
using UnityEngine;

public class SistemaSalud : MonoBehaviour, IRecibeDano
{
    [Header("Configuración Base")]
    [SerializeField] private int saludMaxima = 5;
    private int saludActual;

    public int SaludActual => saludActual;
    public int SaludMaxima => saludMaxima;

    // --- EVENTOS (La clave del desacoplamiento) ---
    // Otros scripts (como la UI de corazones o el Animator del enemigo) se suscribirán aquí
    public event Action<int, int> AlCambiarSalud; // Pasa la salud actual y la máxima
    public event Action AlMorir;

    private bool estaMuerto = false;

    private void Awake()
    {
        // Al nacer, nos curamos al máximo
        saludActual = saludMaxima;
    }

    // Cumplimos el contrato de la interfaz IRecibeDano
    public void RecibirDano(int cantidad)
    {
        if (estaMuerto) return; // Si ya está muerto, ignoramos el daño extra

        saludActual -= cantidad;

        // Evitamos que la vida baje de cero
        saludActual = Mathf.Max(saludActual, 0);

        // Gritamos a los cuatro vientos que nuestra salud ha cambiado
        AlCambiarSalud?.Invoke(saludActual, saludMaxima);

        if (saludActual == 0)
        {
            Morir();
        }
    }

    public void Curar(int cantidad)
    {
        if (estaMuerto) return;

        saludActual += cantidad;

        // Evitamos sobrecurar por encima del máximo
        saludActual = Mathf.Min(saludActual, saludMaxima);

        AlCambiarSalud?.Invoke(saludActual, saludMaxima);
    }

    public void RestaurarSalud(int cantidad)
    {
        saludActual = Mathf.Clamp(cantidad, 0, saludMaxima);
        estaMuerto = saludActual <= 0;
        AlCambiarSalud?.Invoke(saludActual, saludMaxima);
    }

    private void Morir()
    {
        estaMuerto = true;
        // Avisamos de que este objeto acaba de morir
        AlMorir?.Invoke();
    }
}