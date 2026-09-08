using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventario/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [Header("Lista de todos los objetos existentes en el juego")]
    // 1. Inicializamos la lista por defecto para evitar nulos al crear el archivo
    public List<ItemData> itemsDisponibles = new List<ItemData>(); 

    private Dictionary<string, ItemData> diccionarioItems;

    private void OnEnable()
    {
        InicializarDiccionario();
    }

    private void InicializarDiccionario()
    {
        diccionarioItems = new Dictionary<string, ItemData>();

        // 2. PROTECCIÓN: Si la lista sigue siendo nula por algún motivo, abortamos.
        if (itemsDisponibles == null) return; 

        foreach (ItemData item in itemsDisponibles)
        {
            if (item != null && !diccionarioItems.ContainsKey(item.name))
            {
                diccionarioItems.Add(item.name, item);
            }
        }
    }

    public ItemData ObtenerItemPorNombre(string nombre)
    {
        if (diccionarioItems == null || itemsDisponibles == null || diccionarioItems.Count != itemsDisponibles.Count)
        {
            InicializarDiccionario();
        }

        if (diccionarioItems != null && diccionarioItems.TryGetValue(nombre, out ItemData itemEncontrado))
        {
            return itemEncontrado;
        }

        Debug.LogWarning("El ítem '" + nombre + "' no existe en la base de datos.");
        return null;
    }
}