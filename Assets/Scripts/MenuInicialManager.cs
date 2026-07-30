using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicialManager : MonoBehaviour
{
    [Header("Paneles de la Interfaz")]
    [SerializeField] private GameObject panelBotones;
    [SerializeField] private GameObject panelOpciones;
    [SerializeField] private GameObject panelTrofeos;

    private void Start()
    {
        // En el menú inicial necesitamos que el ratón esté libre y visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Nos aseguramos de empezar en la pantalla correcta
        VolverAlMenuPrincipal();
    }

    // Función que llamará el botón "Jugar"
    public void BotonJugar()
    {
        // IMPORTANTE: El texto debe coincidir EXACTAMENTE con el nombre de tu escena.
        SceneManager.LoadScene("Mundo1");
    }

    // Función que llamará el botón "Opciones"
    public void BotonOpciones()
    {
        panelBotones.SetActive(false);
        panelOpciones.SetActive(true);
        panelTrofeos.SetActive(false);
    }

    // Función que llamará el botón "Trofeos"
    public void BotonTrofeos()
    {
        panelBotones.SetActive(false);
        panelOpciones.SetActive(false);
        panelTrofeos.SetActive(true);
    }

    // Función para volver al inicio desde Opciones o Trofeos
    public void VolverAlMenuPrincipal()
    {
        panelBotones.SetActive(true);
        panelOpciones.SetActive(false);
        panelTrofeos.SetActive(false);
    }
}