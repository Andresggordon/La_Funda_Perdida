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
    public int tipoPerspectiva; // Por ejemplo: 0 = Primera persona, 1 = Tercera persona, etc.

    // --- DATOS DE LA MASCOTA ---
    public string nombreMascota;
    public Vector3 posicionMascota;

    // --- INVENTARIO ---
    public List<string> objetosInventario;
    public List<string> objetosDestruidosUID; // Guardará identificadores únicos de los objetos cogidos
    public List<string> nombresObjetosInventario;  // Guardaremos los nombres de los objetos que tienes en el inventario

    // --- CONSTRUCTOR: VALORES POR DEFECTO ---
    public DatosPartida()
    {
        corazonesJugador = 4;
        // Pon aquí unas coordenadas aproximadas de donde está tu terreno
        posicionJugador = new Vector3(50.8f, 71.2f, 93.7f);

        posicionCamara = Vector3.zero;
        tipoPerspectiva = 1;

        nombreMascota = "Fifi";
        posicionMascota = new Vector3(1f, 0f, 0f);

        objetosInventario = new List<string>();
        objetosDestruidosUID = new List<string>();

        nombresObjetosInventario = new List<string>();
    }
}