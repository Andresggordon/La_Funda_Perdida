using UnityEngine;

public class RutaAutobus : MonoBehaviour
{
    [Header("Ruta y Estaciones")]
    public Transform[] paradas; // Arrastra aquí los objetos vacíos del mapa
    public float velocidad = 5f;
    public float tiempoEsperaEstacion = 5f;

    private int indiceActual = 0;
    private float temporizador = 0f;
    private bool esperando = false;

    private void Update()
    {
        if (paradas.Length == 0) return;

        if (esperando)
        {
            temporizador += Time.deltaTime;
            if (temporizador >= tiempoEsperaEstacion)
            {
                esperando = false;
                indiceActual = (indiceActual + 1) % paradas.Length; // Pasa al siguiente o vuelve al inicio
            }
            return;
        }

        Transform destino = paradas[indiceActual];
        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

        // El autobús mira hacia donde va
        Vector3 direccion = destino.position - transform.position;
        if (direccion != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direccion), Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, destino.position) < 0.1f)
        {
            esperando = true;
            temporizador = 0f;
        }
    }
}