// ==========================================
// SCRIPT: DatosPartida.cs
// ==========================================
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatosPartida
{
    // --- CONTROL DE PARTIDA ---
    public bool esPartidaNueva; // ¡AÑADIDO! Para saber si acabamos de empezar

    public int corazonesJugador;
    public Vector3 posicionJugador;
    public Vector3 posicionCamara;
    public int tipoPerspectiva;
    public string nombreMascota;
    public Vector3 posicionMascota;

    public List<string> objetosInventario;
    public List<string> objetosDestruidosUID;
    public List<string> nombresObjetosInventario;
    public List<string> entidadesDerrotadasUID;
    public List<string> trofeosDesbloqueadosID;

    // --- NUEVO: TIEMPO DEL MUNDO ---
    public float horaDelMundo;

    public DatosPartida()
    {
        // Por defecto, toda partida recién creada es "Nueva"
        esPartidaNueva = true; 

        corazonesJugador = 5;
        posicionJugador = new Vector3(50.8f, 71.2f, 93.7f);
        posicionCamara = Vector3.zero;
        tipoPerspectiva = 1;
        nombreMascota = "E";
        posicionMascota = new Vector3(1f, 0f, 0f);

        objetosInventario = new List<string>();
        objetosDestruidosUID = new List<string>();
        nombresObjetosInventario = new List<string>();
        entidadesDerrotadasUID = new List<string>();
        trofeosDesbloqueadosID = new List<string>();

        // Valor por defecto (ej: 8.0f simulando las 8:00 AM)
        horaDelMundo = 8.0f; 
    }
}