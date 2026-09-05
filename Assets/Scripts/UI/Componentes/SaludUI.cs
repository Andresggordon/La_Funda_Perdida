using UnityEngine;
using UnityEngine.UI;

public class SaludUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private SistemaSalud sistemaSalud;
    [SerializeField] private Sprite corazonLleno;
    [SerializeField] private Sprite corazonVacio;
    [SerializeField] private Image[] iconosCorazones;

    private void OnEnable()
    {
        if (sistemaSalud == null) return;

        sistemaSalud.AlCambiarSalud += ActualizarCorazones;
        ActualizarCorazones(sistemaSalud.SaludActual, sistemaSalud.SaludMaxima);
    }

    private void OnDisable()
    {
        if (sistemaSalud == null) return;

        sistemaSalud.AlCambiarSalud -= ActualizarCorazones;
    }

    private void ActualizarCorazones(int saludActual, int saludMaxima)
    {
        if (iconosCorazones == null) return;

        for (int i = 0; i < iconosCorazones.Length; i++)
        {
            Image icono = iconosCorazones[i];
            if (icono == null) continue;

            bool visible = i < saludMaxima;
            icono.enabled = visible;
            if (!visible) continue;

            icono.sprite = i < saludActual ? corazonLleno : corazonVacio;
        }
    }
}
