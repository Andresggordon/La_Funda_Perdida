using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatosPartida
{
    // --- DATOS DEL JUGADOR ---
    public int corazonesJugador;
    public Vector3 posicionJugador;

    // --- DATOS DE LA CÁMARA Y PERSPECTIVA ---
    public Vector3 posicionCamara;
    public int tipoPerspectiva;

    // --- DATOS DE LA MASCOTA ---
    public string nombreMascota;
    public Vector3 posicionMascota;

    // --- INVENTARIO ---
    public List<string> objetosInventario;
    public List<string> objetosDestruidosUID;
    public List<string> nombresObjetosInventario;

    // --- MUNDO E IA ---
    public List<string> entidadesDerrotadasUID;

    // --- ENTORNO (¡NUEVO!) ---
    public float horaDelMundo;

    // --- CONSTRUCTOR: VALORES POR DEFECTO ---
    public DatosPartida()
    {
        corazonesJugador = 4;
        posicionJugador = new Vector3(50.8f, 71.2f, 93.7f);

        posicionCamara = Vector3.zero;
        tipoPerspectiva = 1;

        nombreMascota = "Fifi";
        posicionMascota = new Vector3(1f, 0f, 0f);

        objetosInventario = new List<string>();
        objetosDestruidosUID = new List<string>();
        nombresObjetosInventario = new List<string>();
        entidadesDerrotadasUID = new List<string>();

        // Por defecto, si empezamos una partida nueva, será a mediodía
        horaDelMundo = 12f;
    }
}