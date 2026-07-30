using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuAjustes : MonoBehaviour
{
    [Header("Referencias de Interfaz (Visuales)")]
    public Slider sliderFOV;
    public TextMeshProUGUI textoValorFOV;

    public Slider sliderSensibilidad;
    public TextMeshProUGUI textoValorSensibilidad;

    [Header("Referencias de Interfaz (Audio y Brillo)")]
    public Slider sliderVolMaster;
    public TextMeshProUGUI textoValorVolMaster;

    public Slider sliderVolSFX;
    public TextMeshProUGUI textoValorVolSFX;

    public Slider sliderBrillo;
    public TextMeshProUGUI textoValorBrillo;
    public Image capaBrilloUI;

    public AudioMixer mezcladorDeAudio;

    [Header("Referencias del Mundo (Nulas en Menú)")]
    public Camera camaraPrincipal;
    public JugadorCamara jugadorCamara;

    private void Start()
    {
        // 1. CARGAR DATOS (Si no hay nada guardado, por defecto será 100 o el valor seguro)
        float fov = PlayerPrefs.GetFloat("ajustes_fov", 60f);
        float sens = PlayerPrefs.GetFloat("ajustes_sensibilidad", 50f);
        float volMaster = PlayerPrefs.GetFloat("ajustes_volMaster", 100f);
        float volSFX = PlayerPrefs.GetFloat("ajustes_volSFX", 100f);
        float brillo = PlayerPrefs.GetFloat("ajustes_brillo", 100f);

        // 2. CONFIGURAR INTERFAZ Y TEXTOS
        ConfigurarSlider(sliderFOV, fov, CambiarFOV);
        ActualizarTexto(textoValorFOV, fov, "0");

        ConfigurarSlider(sliderSensibilidad, sens, CambiarSensibilidadRaton);
        ActualizarTexto(textoValorSensibilidad, sens, "0");

        ConfigurarSlider(sliderVolMaster, volMaster, CambiarVolumenMaster);
        ActualizarTexto(textoValorVolMaster, volMaster, "0");

        ConfigurarSlider(sliderVolSFX, volSFX, CambiarVolumenSFX);
        ActualizarTexto(textoValorVolSFX, volSFX, "0");

        ConfigurarSlider(sliderBrillo, brillo, CambiarBrillo);
        ActualizarTexto(textoValorBrillo, brillo, "0");

        // 3. APLICAR AL ENTORNO
        if (camaraPrincipal != null) camaraPrincipal.fieldOfView = fov;
        if (jugadorCamara != null) jugadorCamara.CambiarSensibilidad(sens);

        CambiarVolumenMaster(volMaster);
        CambiarVolumenSFX(volSFX);
        CambiarBrillo(brillo);
    }

    // --- FUNCIONES DE AUDIO ---
    public void CambiarVolumenMaster(float valor)
    {
        float valorNormalizado = Mathf.Clamp(valor / 100f, 0.0001f, 1f);
        if (mezcladorDeAudio != null)
        {
            mezcladorDeAudio.SetFloat("VolumenMaster", Mathf.Log10(valorNormalizado) * 20f);
        }

        ActualizarTexto(textoValorVolMaster, valor, "0");
        PlayerPrefs.SetFloat("ajustes_volMaster", valor);
        PlayerPrefs.Save(); // Obliga a Unity a guardar en disco inmediatamente
    }

    public void CambiarVolumenSFX(float valor)
    {
        float valorNormalizado = Mathf.Clamp(valor / 100f, 0.0001f, 1f);

        if (mezcladorDeAudio != null)
        {
            float decibelios = Mathf.Log10(valorNormalizado) * 20f;
            mezcladorDeAudio.SetFloat("VolumenSFX", decibelios);
        }
        else
        {
            Debug.LogWarning("¡El AudioMixer en el script está vacío (Null)!");
        }

        ActualizarTexto(textoValorVolSFX, valor, "0");
        PlayerPrefs.SetFloat("ajustes_volSFX", valor);
        PlayerPrefs.Save();
    }

    // --- FUNCIÓN DE BRILLO ---
    public void CambiarBrillo(float valor)
    {
        if (capaBrilloUI != null)
        {
            float valorNormalizado = valor / 100f;
            float alpha = 0.8f - (valorNormalizado * 0.8f);
            Color colorCapa = capaBrilloUI.color;
            colorCapa.a = alpha;
            capaBrilloUI.color = colorCapa;
        }

        ActualizarTexto(textoValorBrillo, valor, "0");
        PlayerPrefs.SetFloat("ajustes_brillo", valor);
        PlayerPrefs.Save();
    }

    // --- RESTO DE FUNCIONES ---
    public void CambiarFOV(float valor)
    {
        if (camaraPrincipal != null) camaraPrincipal.fieldOfView = valor;
        ActualizarTexto(textoValorFOV, valor, "0");
        PlayerPrefs.SetFloat("ajustes_fov", valor);
        PlayerPrefs.Save();
    }

    public void CambiarSensibilidadRaton(float valor)
    {
        if (jugadorCamara != null) jugadorCamara.CambiarSensibilidad(valor);
        ActualizarTexto(textoValorSensibilidad, valor, "0");
        PlayerPrefs.SetFloat("ajustes_sensibilidad", valor);
        PlayerPrefs.Save();
    }

    private void ConfigurarSlider(Slider slider, float valor, UnityEngine.Events.UnityAction<float> funcion)
    {
        if (slider != null)
        {
            slider.value = valor;
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(funcion);
        }
    }

    private void ActualizarTexto(TextMeshProUGUI elemento, float valor, string formato)
    {
        if (elemento != null) elemento.text = valor.ToString(formato);
    }
}