using UnityEngine;

public class JugadorCamara : MonoBehaviour
{
    [Header("Referencias (¡Arrastrar desde Unity!)")]
    [SerializeField] private Transform cuelloCamara;
    [SerializeField] private Transform camaraJugador;

    [Header("Ajustes de Visión")]
    [SerializeField] private float sensibilidad = 80f;

    [Header("Sistema de Cámaras")]
    [SerializeField] private Vector3 posPrimeraPersona = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 posTerceraPersona = new Vector3(1.2f, 2f, -5f);
    [SerializeField] private Vector3 posVistaFrontal = new Vector3(0f, 0.3f, 5f);

    private float rotacionX = 0f;
    public int estadoCamara = 0;

    // Hemos eliminado la variable 'controles', el 'Awake', el 'OnEnable' y el 'OnDisable'

    private void Start()
    {
        if (camaraJugador != null)
        {
            camaraJugador.localPosition = posPrimeraPersona;
            camaraJugador.localRotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return; // Si hay pausa, la cámara no gira

        ManejarVistas();
        ManejarRotacion();
    }

    private void ManejarVistas()
    {
        // Leemos el botón de cambiar cámara desde el InputManager centralizado
        if (InputManager.Instancia.controles.Jugador.CambiarCamara.WasPressedThisFrame())
        {
            if (camaraJugador == null) return;

            estadoCamara++;
            if (estadoCamara > 2) estadoCamara = 0;

            switch (estadoCamara)
            {
                case 0:
                    camaraJugador.localPosition = posPrimeraPersona;
                    camaraJugador.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case 1:
                    camaraJugador.localPosition = posTerceraPersona;
                    camaraJugador.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case 2:
                    camaraJugador.localPosition = posVistaFrontal;
                    camaraJugador.localRotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
            }
        }
    }

    private void ManejarRotacion()
    {
        // Leemos el movimiento del ratón desde el InputManager centralizado
        Vector2 inputMirar = InputManager.Instancia.controles.Jugador.Mirar.ReadValue<Vector2>();

        float ratonX = inputMirar.x * sensibilidad * Time.deltaTime;
        float ratonY = inputMirar.y * sensibilidad * Time.deltaTime;

        rotacionX -= ratonY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        if (cuelloCamara != null)
        {
            cuelloCamara.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        }

        transform.Rotate(Vector3.up * ratonX); // Gira el cuerpo entero a los lados
    }

    // Función pública para que el menú pueda modificar la sensibilidad
    public void CambiarSensibilidad(float nuevaSensibilidad)
    {
        sensibilidad = nuevaSensibilidad;
    }
}