using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JugadorMovimiento : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    [SerializeField] private float velocidad = 6.0f;
    [SerializeField] private float gravedad = -10f;
    [SerializeField] private float alturaSalto = 1.5f;

    private CharacterController controller;
    private ControlesJugador controles;
    private Vector3 velocidadVertical;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controles = new ControlesJugador();
    }

    private void OnEnable() { controles.Enable(); }
    private void OnDisable() { controles.Disable(); }

    private void Update()
    {
        // Truco profesional: Si el tiempo está a 0 (pausa), salimos del Update y no nos movemos
        if (Time.timeScale == 0f) return;

        ManejarGravedadYSalto();
        ManejarMovimiento();
    }

    private void ManejarGravedadYSalto()
    {
        if (controller.isGrounded)
        {
            if (velocidadVertical.y < 0) velocidadVertical.y = -2f;

            if (controles.Jugador.Saltar.triggered)
            {
                velocidadVertical.y = Mathf.Sqrt(alturaSalto * -2f * gravedad);
            }
        }
        velocidadVertical.y += gravedad * Time.deltaTime;
    }

    private void ManejarMovimiento()
    {
        Vector2 inputMovimiento = controles.Jugador.Mover.ReadValue<Vector2>();
        Vector3 moverHorizontal = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;

        Vector3 movimientoFinal = moverHorizontal * velocidad;
        movimientoFinal.y = velocidadVertical.y;

        controller.Move(movimientoFinal * Time.deltaTime);
    }
}