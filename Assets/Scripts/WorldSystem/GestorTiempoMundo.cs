// ==========================================
// SCRIPT: GestorTiempoMundo.cs
// UBICACIÓN: Scripts/WorldSystem/
// ==========================================
using System;
using System.Collections;
using UnityEngine;

public class GestorTiempoMundo : MonoBehaviour
{
    public static GestorTiempoMundo Instancia { get; private set; }

    [Header("Referencias")]
    [Tooltip("Arrastra aquí el Directional Light que hace de Sol")]
    [SerializeField] private Light luzSol;

    [Header("Configuración del Tiempo")]
    [Range(0, 24)] public float horaActual = 12f; // 12:00 PM por defecto

    [Tooltip("¿Cuántos minutos del juego pasan por cada segundo real? (Ej: 60 = 1 hora in-game por minuto real)")]
    public float multiplicadorTiempo = 60f;

    // --- EVENTOS DESACOPLADOS ---
    // Otros scripts (farolas que se encienden, monstruos nocturnos) se suscribirán aquí
    public static event Action AlAmanecer;
    public static event Action AlAnochecer;

    private bool esDeDia = true;
    private bool estaDurmiendo = false;

    private void Awake()
    {
        // Patrón Singleton para acceso global rápido
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
    }

    private void Update()
    {
        // Si el juego está en pausa o Paula está durmiendo (transición rápida), detenemos el reloj normal
        if (Time.timeScale == 0f || estaDurmiendo) return;

        // Calculamos el incremento de tiempo
        float incremento = Time.deltaTime * (multiplicadorTiempo / 3600f) * 24f;
        AvanzarTiempoNormal(incremento);
    }

    private void AvanzarTiempoNormal(float incremento)
    {
        horaActual += incremento;

        if (horaActual >= 24f) horaActual = 0f;

        ActualizarRotacionSol();
        ComprobarEventosDíaNoche();
    }

    private void ActualizarRotacionSol()
    {
        if (luzSol == null) return;

        // Mapeamos la hora a grados de rotación:
        // 6 AM = 0º (Amanecer), 12 PM = 90º (Mediodía), 6 PM = 180º (Ocaso)
        float anguloRotacion = (horaActual - 6f) / 24f * 360f;

        // Inclinamos el sol -30 en Y para que las sombras tengan un ángulo más estético
        luzSol.transform.rotation = Quaternion.Euler(anguloRotacion, -30f, 0f);
    }

    private void ComprobarEventosDíaNoche()
    {
        if (esDeDia && (horaActual >= 18f || horaActual < 6f)) // Anochece a las 18:00
        {
            esDeDia = false;
            AlAnochecer?.Invoke();
        }
        else if (!esDeDia && horaActual >= 6f && horaActual < 18f) // Amanece a las 06:00
        {
            esDeDia = true;
            AlAmanecer?.Invoke();
        }
    }

    // --- SISTEMA PARA DORMIR ---
    public void DormirHastaHora(float horaDestino)
    {
        if (!estaDurmiendo)
        {
            StartCoroutine(TransicionDormir(horaDestino));
        }
    }

    // Usamos una corrutina para hacer un "Fast-Forward" visualmente bonito en lugar de un salto brusco
    private IEnumerator TransicionDormir(float horaDestino)
    {
        estaDurmiendo = true;

        float tiempoTransicion = 2.5f; // Lo que tarda la animación en segundos reales
        float temporizador = 0f;
        float horaInicio = horaActual;

        // Ajuste matemático por si dormimos a las 23:00 y despertamos a las 08:00 (cruza la medianoche)
        float horaObjetivoLerp = horaDestino < horaInicio ? horaDestino + 24f : horaDestino;

        while (temporizador < tiempoTransicion)
        {
            temporizador += Time.deltaTime;
            float progreso = temporizador / tiempoTransicion;

            // Aceleración suave
            float progresoSuavizado = Mathf.SmoothStep(0f, 1f, progreso);

            float horaInterpolada = Mathf.Lerp(horaInicio, horaObjetivoLerp, progresoSuavizado);
            horaActual = horaInterpolada % 24f; // Reseteamos si pasa de 24

            ActualizarRotacionSol();
            yield return null;
        }

        horaActual = horaDestino;
        ComprobarEventosDíaNoche();
        estaDurmiendo = false;
    }
}