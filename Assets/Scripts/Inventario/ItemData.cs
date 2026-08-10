using UnityEngine;

// Esta línea es la magia: crea un botón en el menú de Unity para fabricar objetos
[CreateAssetMenu(fileName = "NuevoObjeto", menuName = "Inventario/Nuevo Objeto")]
public class ItemData : ScriptableObject
{
    [Header("Datos Básicos")]
    public string idObjeto; // Un código único, ej: "funda_01" o "pocion_vida"
    public string nombreMostrado; // El nombre que verá el jugador, ej: "Funda de Cuero"

    [TextArea(3, 5)]
    public string descripcion; // Texto que explica qué hace

    public Sprite icono; // La imagen que saldrá en la mochila
    public bool esConsumible; // Para saber si se gasta al usarlo
}