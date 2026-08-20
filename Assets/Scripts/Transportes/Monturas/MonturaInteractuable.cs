using UnityEngine;

// Requiere el script de Outline amarillo[cite: 5] y firma el contrato
[RequireComponent(typeof(ObjetoInteractuable))]
public class MonturaInteractuable : MonoBehaviour, IInteractuable
{
    [Header("Configuración de la Montura")]
    [Tooltip("Crea un GameObject vacío hijo (donde irá el jugador) y arrástralo aquí")]
    public Transform puntoDeMontaje;

    [Tooltip("¿El jugador debe mirar hacia donde mira la montura al subirse?")]
    public bool alinearRotacion = true;

    private bool jugadorSubido = false;
    private GameObject jugadorActual;
    private CharacterController controladorJugador;
    private ObjetoInteractuable interactuable;

    private void Awake()
    {
        interactuable = GetComponent<ObjetoInteractuable>();
    }

    private void Update()
    {
        if (jugadorSubido)
        {
            // Teclado y Mando de PS4 para bajar[cite: 1]
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Jump"))
            {
                Desmontar();
            }
        }
    }

    // La función que nos exige el contrato IInteractuable
    public void EjecutarInteraccion(GameObject jugador)
    {
        if (!jugadorSubido)
        {
            Montar(jugador);
        }
    }

    private void Montar(GameObject jugador)
    {
        jugadorSubido = true;
        jugadorActual = jugador;
        controladorJugador = jugador.GetComponent<CharacterController>();

        // 1. Inmovilizamos a Paula para que la montura tome el control[cite: 1]
        if (controladorJugador != null) controladorJugador.enabled = false;

        // 2. Apagamos el resaltado amarillo[cite: 5]
        interactuable.DesactivarResaltado();

        // 3. Ocultamos la montura del Raycast cambiando su capa temporalmente
        gameObject.layer = LayerMask.NameToLayer("Default");

        // 4. Emparentamos físicamente al jugador
        jugadorActual.transform.SetParent(puntoDeMontaje);
        jugadorActual.transform.localPosition = Vector3.zero;

        if (alinearRotacion)
        {
            jugadorActual.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Desmontar()
    {
        jugadorSubido = false;

        // 1. Soltamos al jugador
        jugadorActual.transform.SetParent(null);

        // 2. Devolvemos el control físico a Paula
        if (controladorJugador != null) controladorJugador.enabled = true;

        // 3. Restauramos la capa para volver a interactuar
        gameObject.layer = LayerMask.NameToLayer("Interactuable");

        // Separación de seguridad para no quedar atrapados
        jugadorActual.transform.position += Vector3.up * 1.5f;
        jugadorActual = null;
    }
}