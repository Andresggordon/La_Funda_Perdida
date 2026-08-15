using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Mis Bolsillos")]
    public int capacidadTotal = 24; // 4 de Hotbar + 20 de mochila
    public ItemData[] slots; // Array de tamaño fijo

    [Header("Estado del Mundo")]
    public List<string> objetosDestruidosUID = new List<string>();

    private void Awake()
    {
        // Inicializamos la mochila vacía
        slots = new ItemData[capacidadTotal];
    }

    public void AnadirObjeto(ItemData nuevoObjeto)
    {
        // Buscamos el primer hueco libre (null)
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = nuevoObjeto;
                Debug.Log("Has recogido: " + nuevoObjeto.nombreMostrado + " en el slot " + i);
                return; // Salimos de la función al guardarlo
            }
        }
        Debug.LogWarning("¡El inventario está lleno!");
    }

    

    // Función modular para intercambiar dos huecos (incluso si uno está vacío)
    public void IntercambiarSlots(int indiceA, int indiceB)
    {
        ItemData objetoTemporal = slots[indiceA];
        slots[indiceA] = slots[indiceB];
        slots[indiceB] = objetoTemporal;
    }
}