using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventario/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [Header("Lista de todos los objetos existentes en el juego")]
    public List<ItemData> itemsDisponibles;

    // Diccionario interno para búsquedas instantáneas
    private Dictionary<string, ItemData> diccionarioItems;

    private void OnEnable()
    {
        InicializarDiccionario();
    }

    private void InicializarDiccionario()
    {
        diccionarioItems = new Dictionary<string, ItemData>();

        foreach (ItemData item in itemsDisponibles)
        {
            // Evitamos errores si hay huecos vacíos o elementos duplicados por accidente
            if (item != null && !diccionarioItems.ContainsKey(item.name))
            {
                diccionarioItems.Add(item.name, item);
            }
        }
    }

    // Función optimizada para buscar un objeto por su nombre
    public ItemData ObtenerItemPorNombre(string nombre)
    {
        // Seguro de vida: Si el diccionario no está listo, lo inicializamos
        if (diccionarioItems == null || diccionarioItems.Count != itemsDisponibles.Count)
        {
            InicializarDiccionario();
        }

        // TryGetValue encuentra el ítem al instante sin recorrer listas
        if (diccionarioItems.TryGetValue(nombre, out ItemData itemEncontrado))
        {
            return itemEncontrado;
        }

        Debug.LogWarning("El ítem '" + nombre + "' no existe en la base de datos ItemDatabase.");
        return null;
    }
}