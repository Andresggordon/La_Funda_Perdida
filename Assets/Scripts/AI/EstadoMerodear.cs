// ==========================================
// SCRIPT: EstadoMerodear.cs
// ==========================================
using UnityEngine;
using UnityEngine.AI;

public class EstadoMerodear : EstadoBase
{
    private float radioDePaseo = 10f;
    private float tiempoDeEspera = 2f;
    private float temporizador = 0f;
    private bool estaEsperando = false;

    public override void Entrar(MaquinaDeEstados maquina)
    {
        // Al entrar a este estado, buscamos un punto inmediatamente
        BuscarNuevoDestino(maquina);
    }

    public override void Actualizar(MaquinaDeEstados maquina)
    {
        // 1. Si estamos esperando, contamos el tiempo
        if (estaEsperando)
        {
            temporizador += Time.deltaTime;
            if (temporizador >= tiempoDeEspera)
            {
                estaEsperando = false;
                BuscarNuevoDestino(maquina);
            }
        }
        // 2. Si no estamos esperando, comprobamos si ya llegamos al destino
        else if (maquina.agenteNavMesh.remainingDistance <= maquina.agenteNavMesh.stoppingDistance)
        {
            // Hemos llegado. Toca esperar un poco antes de volver a moverse.
            estaEsperando = true;
            temporizador = 0f;
        }
    }

    public override void Salir(MaquinaDeEstados maquina)
    {
        // Si por algún motivo salimos de este estado (ej: el jugador nos asusta),
        // reseteamos la ruta para que el mob se frene en seco.
        maquina.agenteNavMesh.ResetPath();
    }

    private void BuscarNuevoDestino(MaquinaDeEstados maquina)
    {
        // Generamos un punto aleatorio dentro de una esfera imaginaria
        Vector3 direccionAleatoria = Random.insideUnitSphere * radioDePaseo;
        direccionAleatoria += maquina.transform.position;

        NavMeshHit impactoNav;

        // SamplePosition busca el punto pisable del NavMesh más cercano a esa posición aleatoria
        if (NavMesh.SamplePosition(direccionAleatoria, out impactoNav, radioDePaseo, NavMesh.AllAreas))
        {
            maquina.agenteNavMesh.SetDestination(impactoNav.position);
        }
    }
}