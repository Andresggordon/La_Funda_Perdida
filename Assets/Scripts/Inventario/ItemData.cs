using UnityEngine;

[CreateAssetMenu(fileName = "NuevoObjeto", menuName = "Inventario/Nuevo Objeto Base")]
public class ItemData : ScriptableObject
{
    [Header("Datos Básicos")]
    public string idObjeto;
    public string nombreMostrado;

    [TextArea(3, 5)]
    public string descripcion;

    public Sprite icono;
    public bool esConsumible;

    [Tooltip("El modelo 3D que aparecerá en la mano o en el suelo al tirarlo")]
    public GameObject prefabMundo;

    [Tooltip("Ajuste fino de la posición en la mano (X, Y, Z)")]
    public Vector3 offsetPosicion;

    [Tooltip("Ajuste fino de la rotación en la mano (X, Y, Z)")]
    public Vector3 offsetRotacion;

    // Este método virtual permite que cada tipo de objeto decida qué hacer al usarse
    public virtual bool Usar(GameObject jugador)
    {
        Debug.Log("Has interactuado con: " + nombreMostrado);
        // Devuelve false por defecto indicando que no se gastó
        return false;
    }
}