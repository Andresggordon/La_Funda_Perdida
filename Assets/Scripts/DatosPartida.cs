using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatosPartida
{
    // --- DATOS DEL JUGADOR ---
    public int corazonesJugador; // Ahora es un número entero
    public Vector3 posicionJugador;

    // --- DATOS DE LA MASCOTA ---
    public string nombreMascota;
    public Vector3 posicionMascota;

    // --- INVENTARIO ---
    public List<string> objetosInventario;

    // --- CONSTRUCTOR: VALORES POR DEFECTO ---
    public DatosPartida()
    {
        corazonesJugador = 4; // El jugador empezará su partida con 4 corazones
        posicionJugador = Vector3.zero;

        nombreMascota = "Fifi";
        posicionMascota = new Vector3(1f, 0f, 0f);

        objetosInventario = new List<string>();
    }
}