// ==========================================
// SCRIPT: GestorTiempoMundo.cs
// UBICACION: Scripts/WorldSystem/
//
// Unica fuente de verdad del tiempo del mundo. Avanza la hora y rota
// el Sol/Luna. TODA la parte visual (luces, skybox, niebla) la aplica
// SkyLightingController, leyendo "horaActual" desde aqui cada frame.
//
// No debe haber ningun otro script rotando Sol/Luna ni modificando
// RenderSettings a la vez que este (por eso se quitaron DayNightCycle
// y TimeManager).
// ==========================================
using System.Collections;
using UnityEngine;

public class GestorTiempoMundo : MonoBehaviour
{
    public static GestorTiempoMundo Instancia { get; private set; }

    [Header("Referencias de Escena")]
    public Light luzSol;
    public Light luzLuna;
    [Tooltip("Opcional: solo si tienes un objeto (domo, estrellas, etc) que necesites rotar aparte del material del skybox")]
    public Transform pivoteCielo;

    [Header("Tiempo")]
    [Range(0, 24)] public float horaActual = 12f;
    [Tooltip("1 segundo real = X segundos de juego")]
    public float multiplicadorTiempo = 60f;

    [Header("Orbita")]
    [Tooltip("Hora a la que el sol esta justo en el horizonte, saliendo")]
    public float horaAmanecer = 6f;
    [Tooltip("Inclinacion del eje de la orbita respecto al horizonte")]
    public float inclinacionOrbita = -30f;

    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        horaActual += Time.deltaTime * (multiplicadorTiempo / 3600f) * 24f;
        if (horaActual >= 24f) horaActual -= 24f;

        ActualizarRotacion();
    }

    void ActualizarRotacion()
    {
        float angulo = (horaActual - horaAmanecer) / 24f * 360f;

        if (luzSol != null) luzSol.transform.rotation = Quaternion.Euler(angulo, inclinacionOrbita, 0f);
        if (luzLuna != null) luzLuna.transform.rotation = Quaternion.Euler(angulo + 180f, inclinacionOrbita, 0f);
        if (pivoteCielo != null) pivoteCielo.rotation = Quaternion.Euler(angulo, 0f, 0f);
    }

    // --- Usado por el sistema de guardado (lo conectamos en el siguiente paso) ---
    public void SetTime(float horaGuardada)
    {
        horaActual = Mathf.Repeat(horaGuardada, 24f);
        ActualizarRotacion();
    }

    // --- Mecanica de "dormir hasta una hora" ---
    public void DormirHastaHora(float horaDestino)
    {
        StartCoroutine(TransicionDormir(horaDestino));
    }

    IEnumerator TransicionDormir(float horaDestino)
    {
        float duracion = 2.5f;
        float transcurrido = 0f;
        float inicio = horaActual;
        float objetivo = horaDestino < inicio ? horaDestino + 24f : horaDestino;

        while (transcurrido < duracion)
        {
            transcurrido += Time.deltaTime;
            float progreso = Mathf.SmoothStep(0f, 1f, transcurrido / duracion);
            horaActual = Mathf.Repeat(Mathf.Lerp(inicio, objetivo, progreso), 24f);
            ActualizarRotacion();
            yield return null;
        }

        horaActual = Mathf.Repeat(horaDestino, 24f);
        ActualizarRotacion();
    }
}