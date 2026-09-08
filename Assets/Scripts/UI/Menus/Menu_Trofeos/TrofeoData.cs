using UnityEngine;

[CreateAssetMenu(fileName = "NuevoTrofeo", menuName = "Inventario/Coleccionables/Trofeo Data")]
public class TrofeoData : ScriptableObject
{
    [Header("Información Básica")]
    public string trophyID;
    public string trophyName;

    [TextArea(3, 5)]
    public string description;

    [Header("Representación Visual")]
    public Sprite icono2D;
    public GameObject prefabGaleria3D;
}