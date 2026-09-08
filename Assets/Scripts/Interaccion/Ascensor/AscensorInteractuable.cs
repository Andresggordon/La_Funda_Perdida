using UnityEngine;

public class AscensorInteractuable : MonoBehaviour, IInteractuable
{
    public Animator ascensorAnimator;

    public void EjecutarInteraccion(GameObject jugador)
    {
        Debug.Log("<color=cyan>¡Botón de ascensor pulsado!</color>");
        
        if (ascensorAnimator != null)
        {
            bool estadoActual = ascensorAnimator.GetBool("estaArriba");
            Debug.Log("Cambiando estaArriba a: " + !estadoActual);
            ascensorAnimator.SetBool("estaArriba", !estadoActual);
        }
        else
        {
            Debug.LogError("¡ERROR! El hueco 'Ascensor Animator' está vacío en el Inspector del botón.");
        }
    }
}