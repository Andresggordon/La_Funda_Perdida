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

    // --- CONSTRUCTOR: VALORES POR DEFECTO ---
    public DatosPartida()
    {
        corazonesJugador = 4;
        posicionJugador = Vector3.zero;

        // Valores por defecto para la cámara
        posicionCamara = Vector3.zero;
        tipoPerspectiva = 1; // Por defecto la tercera persona, por ejemplo

        nombreMascota = "Fifi";
        posicionMascota = new Vector3(1f, 0f, 0f);

        objetosInventario = new List<string>();
    }
}