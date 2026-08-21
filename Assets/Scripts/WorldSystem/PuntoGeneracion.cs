// ==========================================
// SCRIPT: PuntoGeneracion.cs
// ==========================================
using UnityEngine;

[RequireComponent(typeof(IdentificadorObjeto))]
public class PuntoGeneracion : MonoBehaviour
{
    [Header("¿Qué vamos a generar aquí?")]
    public GameObject prefabAGenerar;

    // ¡NUEVO! Bandera para saber si el jugador ya limpió esta zona
    public bool enemigoDerrotado { get; private set; } = false;

    private IdentificadorObjeto miID;
    private GameObject entidadGenerada;

    private void Start()
    {
        miID = GetComponent<IdentificadorObjeto>();
        SaveManager saveManager = FindFirstObjectByType<SaveManager>();

        if (saveManager != null && saveManager.ExistePartida())
        {
            DatosPartida datos = saveManager.CargarPartida();

            // Si el ID de este Spawner está en la lista negra, marcamos y abortamos
            if (datos != null && datos.entidadesDerrotadasUID.Contains(miID.idUnico))
            {
                enemigoDerrotado = true;
                return;
            }
        }

        // Si el enemigo sigue vivo en la historia, lo instanciamos
        if (prefabAGenerar != null)
        {
            entidadGenerada = Instantiate(prefabAGenerar, transform.position, transform.rotation);

            // ¡LA CONEXIÓN MÁGICA! 
            // Buscamos si el clon tiene un SistemaSalud y nos suscribimos a su muerte
            if (entidadGenerada.TryGetComponent(out SistemaSalud saludClon))
            {
                saludClon.AlMorir += RegistrarMuerte;
            }
        }
    }

    private void RegistrarMuerte()
    {
        enemigoDerrotado = true;
        Debug.Log("Spawner " + gameObject.name + " registra que su mob ha sido derrotado.");
    }

    // Dibujamos una esfera en el editor. Rojo = Vivo, Verde = Derrotado
    private void OnDrawGizmos()
    {
        Gizmos.color = enemigoDerrotado ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}