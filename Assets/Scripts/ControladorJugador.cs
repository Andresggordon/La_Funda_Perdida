using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    public float velocidad = 6.0f;
    public float gravedad = -9.81f;

    private CharacterController controller;
    private Vector3 velocidadVertical;

    void Start()
    {
        // Buscamos el componente Character Controller que añadiste
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Movimiento Horizontal (WASD / Flechas)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;
        controller.Move(mover * velocidad * Time.deltaTime);

        // 2. Aplicar Gravedad
        // Si estamos en el suelo, la velocidad de caída se resetea
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);

        // Calculamos la caída
        velocidadVertical.y += gravedad * Time.deltaTime;

        // Ejecutamos el movimiento de caída
        controller.Move(velocidadVertical * Time.deltaTime);
    }
}