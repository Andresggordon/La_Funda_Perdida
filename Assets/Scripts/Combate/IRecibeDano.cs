// ==========================================
// SCRIPT: IRecibeDano.cs
// ==========================================
using UnityEngine;

public interface IRecibeDano
{
    // Cualquier objeto que firme este contrato DEBE tener esta función
    void RecibirDano(int cantidad);
}