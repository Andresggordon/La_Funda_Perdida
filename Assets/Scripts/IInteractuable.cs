using UnityEngine;

// Esto no es un MonoBehaviour, es una Interfaz (un contrato)
public interface IInteractuable
{
    // Cualquier script que implemente esto, DEBE tener esta función obligatoriamente
    void EjecutarInteraccion(GameObject jugador);
}