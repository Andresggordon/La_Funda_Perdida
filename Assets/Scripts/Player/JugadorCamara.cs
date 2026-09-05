using UnityEngine;

public class JugadorCamara : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform cuelloCamara;
    [SerializeField] private Transform camaraJugador;

    [Header("Ajustes de Visión")]
    [SerializeField] private float sensibilidad = 80f;

    [Header("Sistema de Cámaras (Offsets relativos al Cuello)")]
    // Quitamos los valores por defecto aquí. Los leeremos del Editor automáticamente.
    [SerializeField] private Vector3 posPrimeraPersona;
    [SerializeField] private Vector3 posTerceraPersona = new Vector3(1.2f, 2f, -5f);
    [SerializeField] private Vector3 posVistaFrontal = new Vector3(0f, 0.3f, 5f);

    private float rotacionX = 0f;
    public int estadoCamara = 0;

    private void Awake()
    {
        // LA MAGIA: Guardamos la posición exacta en la que TÚ dejaste la cámara en el modo Edición.
        // Así nunca saldrá volando ni ignorará tus ajustes manuales.
        if (camaraJugador != null)
        {
            posPrimeraPersona = camaraJugador.localPosition;
        }
    }

    private void Start()
    {
        AplicarEstadoCamara(estadoCamara);
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        ManejarVistas();
        ManejarRotacion();
    }

    private void ManejarVistas()
    {
        if (InputManager.Instancia.controles.Jugador.CambiarCamara.WasPressedThisFrame())
        {
            if (camaraJugador == null) return;

            estadoCamara++;
            if (estadoCamara > 2) estadoCamara = 0;
            AplicarEstadoCamara(estadoCamara);
        }
    }

    private void ManejarRotacion()
    {
        Vector2 inputMirar = InputManager.Instancia.controles.Jugador.Mirar.ReadValue<Vector2>();

        float ratonX = inputMirar.x * sensibilidad * Time.deltaTime;
        float ratonY = inputMirar.y * sensibilidad * Time.deltaTime;

        rotacionX -= ratonY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        if (cuelloCamara != null)
        {
            cuelloCamara.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        }

        transform.Rotate(Vector3.up * ratonX);
    }

    public void AplicarEstadoCamara(int estado)
    {
        estadoCamara = Mathf.Clamp(estado, 0, 2);
        if (camaraJugador == null) return;

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

    public void CambiarSensibilidad(float nuevaSensibilidad)
    {
        sensibilidad = nuevaSensibilidad;
    }
}