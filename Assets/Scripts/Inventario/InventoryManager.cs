using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Mis Bolsillos")]
    public List<ItemData> objetosActuales = new List<ItemData>();

    [Header("Estado del Mundo")]
    // Guardamos aquí los IDs de las cosas que ya hemos destruido
    public List<string> objetosDestruidosUID = new List<string>();

    public void AnadirObjeto(ItemData nuevoObjeto)
    {
        objetosActuales.Add(nuevoObjeto);
        Debug.Log("Has recogido: " + nuevoObjeto.nombreMostrado);
    }

    public void QuitarObjeto(ItemData objetoAQuitar)
    {
        if (objetosActuales.Contains(objetoAQuitar))
        {
            objetosActuales.Remove(objetoAQuitar);
        }
    }
}