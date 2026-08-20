using UnityEngine;

[RequireComponent(typeof(ObjetoInteractuable))]
[RequireComponent(typeof(Flotabilidad))]
[RequireComponent(typeof(Rigidbody))]
public class ControladorPaddleSurf : MonoBehaviour
{
    [Header("Configuración de Montura")]
    [Tooltip("Crea un GameObject vacío hijo de la tabla (donde irán los pies de Paula) y arrástralo aquí")]
    public Transform puntoDeMontaje;

    private bool jugadorSubido = false;
    private GameObject jugadorActual;
    private CharacterController controladorJugador;
    private ObjetoInteractuable interactuable;
    private Rigidbody rb;

    private void Awake()
    {
        // Cacheamos las referencias al inicio (Buenas prácticas)
        interactuable = GetComponent<ObjetoInteractuable>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (jugadorSubido)
        {
            // Escuchamos el input dual (Teclado o Mando PS4) para desmontar
            // Usamos tu InputManager o el input clásico temporalmente
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Jump"))
            {
                DesmontarTabla();
            }
        }
    }

    // Tu script central de Raycast del jugador llamará a esta función al pulsar la 'E' / Cuadrado
    public void EjecutarInteraccion(GameObject jugador)
    {
        if (!jugadorSubido)
        {
            MontarTabla(jugador);
        }
    }

    private void MontarTabla(GameObject jugador)
    {
        jugadorSubido = true;
        jugadorActual = jugador;
        controladorJugador = jugador.GetComponent<CharacterController>();

        // 1. Inmovilizamos a Paula apagando su controlador físico para que actúe como montura
        if (controladorJugador != null) controladorJugador.enabled = false;

        // 2. Apagamos el brillo amarillo manualmente por si acaso
        interactuable.DesactivarResaltado();

        // 3. ¡LA CLAVE! Cambiamos la capa a "Default". 
        // Así el Raycast de Paula ya no lo detecta como interactuable y la 'E' desaparece sola de la pantalla.
        gameObject.layer = LayerMask.NameToLayer("Default");

        // 4. Emparentamos físicamente al jugador encima de la tabla
        jugadorActual.transform.SetParent(puntoDeMontaje);
        jugadorActual.transform.localPosition = Vector3.zero;

        // Mantenemos la rotación en Y de Paula para que mire hacia adelante, pero reseteamos X y Z
        jugadorActual.transform.localRotation = Quaternion.Euler(0, jugadorActual.transform.localEulerAngles.y, 0);
    }

    private void DesmontarTabla()
    {
        jugadorSubido = false;

        // 1. Soltamos al jugador
        jugadorActual.transform.SetParent(null);

        // 2. Le devolvemos el movimiento
        if (controladorJugador != null) controladorJugador.enabled = true;

        // 3. Restauramos la capa a "Interactuable" para poder volver a subirnos luego
        gameObject.layer = LayerMask.NameToLayer("Interactuable");

        // Opcional: Un pequeño empujón hacia arriba/adelante para no bajarnos "dentro" de la tabla
        jugadorActual.transform.position += Vector3.up * 1f;

        jugadorActual = null;
    }
}