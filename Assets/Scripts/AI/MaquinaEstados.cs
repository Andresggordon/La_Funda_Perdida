// ==========================================
// SCRIPT: MaquinaDeEstados.cs
// ==========================================
using UnityEngine;
using UnityEngine.AI;

// Obligamos a que el objeto tenga un NavMeshAgent para poder moverse
[RequireComponent(typeof(NavMeshAgent))]
public class MaquinaDeEstados : MonoBehaviour
{
    private EstadoBase estadoActual;

    // Referencias públicas para que los Estados puedan usarlas
    public NavMeshAgent agenteNavMesh { get; private set; }

    // Aquí puedes añadir referencias al jugador, rangos de visión, etc.

    private void Start()
    {
        // Arrancamos la máquina de estados poniéndola a merodear por defecto
        CambiarEstado(new EstadoMerodear());
    }

    private void Awake()
    {
        agenteNavMesh = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Si tenemos una tarea asignada, la ejecutamos cada frame
        if (estadoActual != null)
        {
            estadoActual.Actualizar(this);
        }
    }

    public void CambiarEstado(EstadoBase nuevoEstado)
    {
        // 1. Salimos del estado anterior limpiamente
        if (estadoActual != null)
        {
            estadoActual.Salir(this);
        }

        // 2. Asignamos el nuevo estado
        estadoActual = nuevoEstado;

        // 3. Entramos en el nuevo estado
        estadoActual.Entrar(this);
    }
}