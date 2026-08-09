using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JugadorMovimiento : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    [SerializeField] private float velocidadCaminar = 6.0f;
    [SerializeField] private float velocidadCorrer = 10.0f;
    [SerializeField] private float gravedad = -10f;
    [SerializeField] private float alturaSalto = 1.5f;

    [Header("Ajustes de Agua")]
    [SerializeField] private float velocidadNadar = 4.0f;
    [SerializeField] private float velocidadAscenso = 4.0f;
    [SerializeField] private float gravedadAgua = -1.5f;
    [SerializeField] private float velocidadBuceandoRapido = -8.0f;

    [Header("Referencias Visuales y Rotación")]
    [SerializeField] private Transform pivotVisual; // Arrastra aquí el objeto PivotVisual
    [SerializeField] private float velocidadRotacionNado = 6.0f;
    private float pitchActual = 0f;

    [Header("Animación")]
    private Animator anim;

    private CharacterController controller;
    private Vector3 velocidadVertical;

    private bool estaNadando = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
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
        // --- LÓGICA SI ESTAMOS EN EL AGUA ---
        if (estaNadando)
        {
            // Usamos IsPressed() para detectar botones mantenidos de forma fiable
            bool pulsandoArriba = InputManager.Instancia.controles.Jugador.Saltar.IsPressed();
            bool pulsandoAbajo = InputManager.Instancia.controles.Jugador.Correr.IsPressed();

            float targetPitch = 0f;

            if (pulsandoArriba)
            {
                velocidadVertical.y = velocidadAscenso;
                targetPitch = -90f; // Mira hacia arriba

                if (anim != null)
                {
                    anim.SetBool("Ascenso", true);
                    anim.SetBool("Buceando", false);
                }
            }
            else if (pulsandoAbajo)
            {
                velocidadVertical.y = velocidadBuceandoRapido;
                targetPitch = 90f; // Mira hacia abajo

                if (anim != null)
                {
                    anim.SetBool("Ascenso", false);
                    anim.SetBool("Buceando", true);
                }
            }
            else
            {
                velocidadVertical.y = gravedadAgua;
                targetPitch = 0f; // Horizontal

                if (anim != null)
                {
                    anim.SetBool("Ascenso", false);
                    anim.SetBool("Buceando", false);
                }
            }

            // Aplicamos la rotación suave al PivotVisual (el Animator ya no interferirá)
            if (pivotVisual != null)
            {
                Vector3 rotEuler = pivotVisual.localEulerAngles;
                pitchActual = Mathf.LerpAngle(pitchActual, targetPitch, Time.deltaTime * velocidadRotacionNado);
                rotEuler.x = pitchActual;
                pivotVisual.localEulerAngles = rotEuler;
            }

            return;
        }

        // --- LÓGICA NORMAL EN TIERRA ---
        if (controller.isGrounded)
        {
            if (velocidadVertical.y < 0) velocidadVertical.y = -2f;

            if (InputManager.Instancia.controles.Jugador.Saltar.triggered)
            {
                velocidadVertical.y = Mathf.Sqrt(alturaSalto * -2f * gravedad);

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
        Vector2 inputMovimiento = InputManager.Instancia.controles.Jugador.Mover.ReadValue<Vector2>();
        bool estaCorriendo = InputManager.Instancia.controles.Jugador.Correr.IsPressed();

        float velocidadActual;
        if (estaNadando)
        {
            velocidadActual = velocidadNadar;
        }
        else
        {
            velocidadActual = estaCorriendo ? velocidadCorrer : velocidadCaminar;
        }

        Vector3 moverHorizontal = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;

        Vector3 movimientoFinal = moverHorizontal * velocidadActual;
        movimientoFinal.y = velocidadVertical.y;

        controller.Move(movimientoFinal * Time.deltaTime);

        if (anim != null)
        {
            float multiplicadorAnim = (estaCorriendo && !estaNadando) ? 2f : 1f;

            anim.SetFloat("VelocidadX", inputMovimiento.x * multiplicadorAnim, 0.1f, Time.deltaTime);
            anim.SetFloat("VelocidadZ", inputMovimiento.y * multiplicadorAnim, 0.1f, Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agua"))
        {
            estaNadando = true;

            if (anim != null) anim.SetBool("Nadando", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Agua"))
        {
            estaNadando = false;

            if (anim != null)
            {
                anim.SetBool("Nadando", false);
                anim.SetBool("Ascenso", false);
                anim.SetBool("Buceando", false);
            }

            // Reseteamos la rotación del pivote al salir
            if (pivotVisual != null)
            {
                pitchActual = 0f;
                Vector3 rotEuler = pivotVisual.localEulerAngles;
                rotEuler.x = 0f;
                pivotVisual.localEulerAngles = rotEuler;
            }

            velocidadVertical.y = Mathf.Sqrt(alturaSalto * -1f * gravedad);
        }
    }
}