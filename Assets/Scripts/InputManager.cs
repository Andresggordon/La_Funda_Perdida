using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Usamos el patrón "Singleton" básico para que cualquier script pueda encontrar este Manager fácilmente
    public static InputManager Instancia { get; private set; }

    public ControlesJugador controles { get; private set; }

    private void Awake()
    {
        // Aseguramos que solo haya un InputManager en toda la escena
        if (Instancia != null && Instancia != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instancia = this;

        // ¡Aquí es el ÚNICO lugar donde creamos los controles!
        controles = new ControlesJugador();
    }

    private void OnEnable()
    {
        controles.Enable();
    }

    private void OnDisable()
    {
        controles.Disable();
    }
}