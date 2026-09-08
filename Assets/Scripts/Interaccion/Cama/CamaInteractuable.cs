// ==========================================
// SCRIPT: CamaInteractuable.cs
// UBICACIÓN: Scripts/Interaccion/
// ==========================================
using UnityEngine;

// 1. Forzamos a que tenga el script de resaltado amarillo (igual que hicimos en ObjetoInteractuable)
[RequireComponent(typeof(ObjetoInteractuable))]
public class CamaInteractuable : MonoBehaviour, IInteractuable // 2. Firmamos el contrato
{
    [Header("Configuración de Descanso")]
    public float horaDespertar = 8f; // Despierta a las 08:00 AM por defecto

    // 3. El contrato nos obliga a tener esta función exacta, que llamará el Raycast del jugador
    public void EjecutarInteraccion(GameObject jugador)
    {
        Debug.Log("Paula se va a dormir...");

        // A. Avanzamos el tiempo de forma cinemática contactando con el Singleton
        if (GestorTiempoMundo.Instancia != null)
        {
            GestorTiempoMundo.Instancia.DormirHastaHora(horaDespertar);
        }

        // B. Curamos a Paula conectándonos a su SistemaSalud[cite: 1]
        if (jugador.TryGetComponent(out SistemaSalud saludJugador))
        {
            // Usamos un número muy alto para curar al 100% (la función ya tiene un Mathf.Min de seguridad)[cite: 1]
            saludJugador.Curar(999);
        }

        // C. (Bonus de Arquitectura) Guardado automático al dormir
        GuardadoEnJuego gestorGuardado = FindFirstObjectByType<GuardadoEnJuego>();
        if (gestorGuardado != null)
        {
            gestorGuardado.Click_GuardarPartida();

            // Opcional: Podrías conectar tu ControladorUI para que muestre el icono de guardado aquí[cite: 1]
            if (FindFirstObjectByType<ControladorUI>() != null)
            {
                FindFirstObjectByType<ControladorUI>().MostrarEfectoGuardado();
            }
        }
    }
}