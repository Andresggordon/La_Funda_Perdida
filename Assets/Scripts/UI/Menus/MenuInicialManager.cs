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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        VolverAlMenuPrincipal();
    }

    public void BotonJugar()
    {
        SceneManager.LoadScene("Mundo1");
    }

    public void BotonOpciones()
    {
        panelBotones.SetActive(false);
        panelOpciones.SetActive(true);
        panelTrofeos.SetActive(false);
    }

    public void BotonTrofeos()
    {
        panelBotones.SetActive(false);
        panelOpciones.SetActive(false);
        panelTrofeos.SetActive(true);
    }

    public void VolverAlMenuPrincipal()
    {
        panelBotones.SetActive(true);
        panelOpciones.SetActive(false);
        panelTrofeos.SetActive(false);
    }
}