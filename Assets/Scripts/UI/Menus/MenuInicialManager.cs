using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicialManager : MonoBehaviour
{
    [Header("Paneles de la Interfaz")]
    [SerializeField] private GameObject panelBotones;
    [SerializeField] private GameObject panelOpciones;
    
    [Tooltip("¡CUIDADO! Arrastra aquí el objeto RAÍZ 'PanelTrofeos', NO el panel de inspección ni el de galería.")]
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
        if (panelBotones != null) panelBotones.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(true);
        if (panelTrofeos != null) panelTrofeos.SetActive(false);
    }

    public void BotonTrofeos()
    {
        // 1. Apagamos el resto del menú
        if (panelBotones != null) panelBotones.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(false);

        // 2. Encendemos el contenedor principal de los trofeos
        if (panelTrofeos != null)
        {
            panelTrofeos.SetActive(true);

            // 3. SEGURIDAD ARQUITECTÓNICA: Forzamos al TrophyManager a resetear su estado
            TrophyManager gestorTrofeos = panelTrofeos.GetComponent<TrophyManager>();
            if (gestorTrofeos != null)
            {
                gestorTrofeos.RegresarAGaleria();
            }
            else
            {
                Debug.LogError("[MenuInicialManager] El 'panelTrofeos' asignado NO tiene el script TrophyManager. ¡Has arrastrado el objeto equivocado en el Inspector!");
            }
        }
    }

    public void VolverAlMenuPrincipal()
    {
        if (panelBotones != null) panelBotones.SetActive(true);
        if (panelOpciones != null) panelOpciones.SetActive(false);
        if (panelTrofeos != null) panelTrofeos.SetActive(false);
    }
}