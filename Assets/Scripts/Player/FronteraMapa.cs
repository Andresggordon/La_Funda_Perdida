using UnityEngine;
using UnityEngine.InputSystem; // <-- NUEVO: Necesario para el sistema de controles moderno

public class FronteraMapa : MonoBehaviour
{
    [Header("Narrativa")]
    public PerfilPersonaje perfilPadre;
    [TextArea]
    public string mensajeAdvertencia = "¡Paula, no te alejes tanto de la costa! Vuelve a la isla.";

    [Header("Físicas")]
    public float distanciaRebote = 3f;

    private CharacterController controladorPersonaje;

    private void Start()
    {
        controladorPersonaje = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Forma segura: No da error aunque la etiqueta esté mal registrada.
        // Además, añadimos un "Plan B": si el objeto contiene la palabra "Muro" en su nombre, también funciona.
        if (other.gameObject.tag == "LimiteMundo" || other.gameObject.name.Contains("Muro"))
        {
            // 1. Mostrar la interfaz
            GestorDialogos.Instancia.MostrarMensaje(perfilPadre, mensajeAdvertencia);

            // 2. Congelar el mundo
            Time.timeScale = 0f;

            // 3. Calcular el rebote (Empujamos a Paula en dirección opuesta al muro)
            Vector3 puntoChoque = other.ClosestPoint(transform.position);
            Vector3 direccionRebote = (transform.position - puntoChoque).normalized;
            direccionRebote.y = 0; // Evitamos que salga volando hacia el cielo

            Vector3 posicionSegura = transform.position + (direccionRebote * distanciaRebote);

            // Si usamos CharacterController, hay que apagarlo un microsegundo para moverlo
            if (controladorPersonaje != null)
            {
                controladorPersonaje.enabled = false;
                transform.position = posicionSegura;
                controladorPersonaje.enabled = true;
            }
            else
            {
                transform.position = posicionSegura;
            }

            // 4. Hacer que Paula mire hacia donde la hemos empujado (hacia la isla)
            if (direccionRebote != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direccionRebote);
            }
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f && GestorDialogos.Instancia.panelUI.activeSelf)
        {
            bool presionado = false;

            if (Keyboard.current != null && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)) presionado = true;
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) presionado = true;
            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) presionado = true;

            if (presionado)
            {
                // Si la máquina de escribir sigue funcionando...
                if (GestorDialogos.Instancia.estaEscribiendo)
                {
                    GestorDialogos.Instancia.CompletarTexto(); // ...lo escribimos de golpe
                }
                else
                {
                    GestorDialogos.Instancia.OcultarMensaje(); // ...si ya terminó, cerramos y seguimos jugando
                    Time.timeScale = 1f; 
                }
            }
        }
    }
}