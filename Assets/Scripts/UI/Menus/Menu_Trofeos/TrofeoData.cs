using UnityEngine;

[CreateAssetMenu(fileName = "NuevoTrofeo", menuName = "Inventario/Nuevo Trofeo")]
public class TrofeoData : ScriptableObject
{
    public string trophyID;
    public string trophyName;
    [TextArea(3, 5)] public string description;
    
    public Sprite icono2D;
    public GameObject prefabGaleria3D;

    [Header("Ajustes de Inspección 3D")]
    [Tooltip("Rotación inicial para que mire a la cámara")]
    public Vector3 rotacionInicial = Vector3.zero;
    [Tooltip("Escala específica para que se vea bien en el menú")]
    public Vector3 escalaInspeccion = Vector3.one; 
}