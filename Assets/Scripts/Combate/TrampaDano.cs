using UnityEngine;

public class TrampaDano : MonoBehaviour
{
    [SerializeField] private int cantidadDano = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IRecibeDano objetivo))
            objetivo.RecibirDano(cantidadDano);
    }
}
