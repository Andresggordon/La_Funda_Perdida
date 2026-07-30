using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Paneles (UI)")]
    public GameObject panelPausaPrincipal;
    public GameObject panelOpciones;

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

        // Nos aseguramos de que los menús empiecen apagados
        if (panelPausaPrincipal != null) panelPausaPrincipal.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(false);
    }

    private void Update()
    {
        // Detecta la tecla de pausa (ESC) con tu sistema de input
        if (controles.Jugador.Pausa.WasPressedThisFrame())
        {
            if (juegoPausado)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    public void PausarJuego() // Era tu AlternarPausa, lo dividimos en dos para que sea más fácil de usar por los botones
    {
        juegoPausado = true;
        Time.timeScale = 0f; // Congela el tiempo
        Cursor.lockState = CursorLockMode.None; // Libera el ratón
        Cursor.visible = true;

        // Enciende el menú principal de pausa y apaga las opciones por si acaso
        if (panelPausaPrincipal != null) panelPausaPrincipal.SetActive(true);
        if (panelOpciones != null) panelOpciones.SetActive(false);
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f; // Reanuda el tiempo
        Cursor.lockState = CursorLockMode.Locked; // Atrapa el ratón
        Cursor.visible = false;

        // Apaga todo
        if (panelPausaPrincipal != null) panelPausaPrincipal.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(false);
    }

    // --- FUNCIONES PARA LOS BOTONES ---

    public void AbrirOpciones()
    {
        panelPausaPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
        panelPausaPrincipal.SetActive(true);
    }
}