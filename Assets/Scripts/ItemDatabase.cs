using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventario/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [Header("Lista de todos los objetos existentes en el juego")]
    public List<ItemData> itemsDisponibles;

    // Función para buscar un objeto por su nombre automáticamente
    public ItemData ObtenerItemPorNombre(string nombre)
    {
        foreach (ItemData item in itemsDisponibles)
        {
            if (item != null && item.name == nombre)
            {
                return item;
            }
        }
        return null;
    }
}