using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPersonaje", menuName = "Dialogos/Perfil de Personaje")]
public class PerfilPersonaje : ScriptableObject
{
    [Header("Datos del Personaje")]
    public string nombre;
    public Sprite retrato;
}