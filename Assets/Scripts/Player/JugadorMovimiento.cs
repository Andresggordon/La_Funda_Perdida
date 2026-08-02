using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JugadorMovimiento : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    [SerializeField] private float velocidadCaminar = 6.0f;
    [SerializeField] private float velocidadCorrer = 10.0f;
    [SerializeField] private float gravedad = -10f;
    [SerializeField] private float alturaSalto = 1.5f;

    [Header("Animación")]
    private Animator anim;

    private CharacterController controller;
    private Vector3 velocidadVertical;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Buscamos el Animator en el hijo (BaseMesh_Chibi)
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        ManejarGravedadYSalto();
        ManejarMovimiento();
    }

    private void ManejarGravedadYSalto()
    {
        if (controller.isGrounded)
        {
            if (velocidadVertical.y < 0) velocidadVertical.y = -2f;

            if (InputManager.Instancia.controles.Jugador.Saltar.triggered)
            {
                // Salto físico
                velocidadVertical.y = Mathf.Sqrt(alturaSalto * -2f * gravedad);

                // Disparamos la animación de salto
                if (anim != null)
                {
                    anim.SetTrigger("Saltar");
                }
            }
        }
        velocidadVertical.y += gravedad * Time.deltaTime;
    }

    private void ManejarMovimiento()
    {
        // 1. Leemos los inputs
        Vector2 inputMovimiento = InputManager.Instancia.controles.Jugador.Mover.ReadValue<Vector2>();
        bool estaCorriendo = InputManager.Instancia.controles.Jugador.Correr.ReadValue<float>() > 0f;

        // 2. Determinamos la velocidad actual
        float velocidadActual = estaCorriendo ? velocidadCorrer : velocidadCaminar;

        // 3. Calculamos la dirección
        Vector3 moverHorizontal = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;

        // 4. Aplicamos la velocidad y el movimiento final
        Vector3 movimientoFinal = moverHorizontal * velocidadActual;
        movimientoFinal.y = velocidadVertical.y;

        controller.Move(movimientoFinal * Time.deltaTime);

        // 5. Pasamos los valores al Animator
        if (anim != null)
        {
            // Si corre, multiplicamos el valor por 2 (para llegar a Y = 2 en el Blend Tree)
            float multiplicadorAnim = estaCorriendo ? 2f : 1f;

            anim.SetFloat("VelocidadX", inputMovimiento.x * multiplicadorAnim, 0.1f, Time.deltaTime);
            anim.SetFloat("VelocidadZ", inputMovimiento.y * multiplicadorAnim, 0.1f, Time.deltaTime);
        }
    }
}