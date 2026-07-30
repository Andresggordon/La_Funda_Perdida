using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Interfaz (UI)")]
    [SerializeField] private GameObject menuPausaUI;

    private ControlesJugador controles;
    private bool juegoPausado = false;

    private void Awake()
    {
        controles = new ControlesJugador();
    }

    private void OnEnable() { controles.Enable(); }
    private void OnDisable() { controles.Disable(); }

    private void Start()
    {
        // Al empezar, bloqueamos el ratón y el tiempo fluye normal
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        if (menuPausaUI != null)
        {
            menuPausaUI.SetActive(false);
        }
    }

    private void Update()
    {
        // El GameManager es el único que escucha la tecla ESC
        if (controles.Jugador.Pausa.WasPressedThisFrame())
        {
            AlternarPausa();
        }
    }

    public void AlternarPausa() // Lo ponemos "public" para que el futuro botón del Canvas pueda usarlo
    {
        juegoPausado = !juegoPausado;

        if (juegoPausado)
        {
            Time.timeScale = 0f; // Congela el tiempo de Unity
            Cursor.lockState = CursorLockMode.None; // Libera el ratón
            Cursor.visible = true;

            //Encender el menu
            if (menuPausaUI != null) menuPausaUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el tiempo
            Cursor.lockState = CursorLockMode.Locked; // Atrapa el ratón
            Cursor.visible = false;
            
            //Apagar el menu
            if (menuPausaUI != null) menuPausaUI.SetActive(false);
        }
    }
}