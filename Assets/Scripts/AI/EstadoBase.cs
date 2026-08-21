// ==========================================
// SCRIPT: EstadoBase.cs
// ==========================================
using UnityEngine;

// No hereda de MonoBehaviour. Es puro C# estándar.
public abstract class EstadoBase
{
    // Se ejecuta una sola vez al adoptar esta actitud
    public abstract void Entrar(MaquinaDeEstados maquina);

    // Se ejecuta cada frame mientras dure esta actitud (su propio Update)
    public abstract void Actualizar(MaquinaDeEstados maquina);

    // Se ejecuta una sola vez al abandonar esta actitud
    public abstract void Salir(MaquinaDeEstados maquina);
}