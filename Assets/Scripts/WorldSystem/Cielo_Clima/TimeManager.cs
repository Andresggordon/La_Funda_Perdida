using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Configuración del Tiempo")]
    [Range(0, 24)] public float currentTime = 12f;
    [Tooltip("1 segundo real = X segundos en el juego")]
    public float timeMultiplier = 60f; 

    // Evento al que se suscribirán otros sistemas (iluminación, NPCs, spawn de enemigos)
    public event Action<float> OnTimePercentChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return; 
        }
        Instance = this;
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        currentTime += Time.deltaTime * (timeMultiplier / 3600f) * 24f;
        if (currentTime >= 24f) currentTime = 0f;

        // Calculamos el porcentaje del día (0.0 a 1.0) y lanzamos el evento
        float timePercent = currentTime / 24f;
        OnTimePercentChanged?.Invoke(timePercent);
    }

    // Método para tu sistema de guardado (SaveSystem)
    public void SetTime(float timeToSet)
    {
        currentTime = timeToSet % 24f;
    }
}