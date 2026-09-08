using UnityEngine;

public class FijarJugador : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador pisa la plataforma, lo convertimos en "hijo" del ascensor
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si el jugador sale de la plataforma, lo soltamos en el mundo
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}