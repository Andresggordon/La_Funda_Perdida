using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Flotabilidad : MonoBehaviour
{
    [Header("Ajustes del Agua")]
    public float nivelDelAgua = 0f; // Altura (Y) del plano de agua en tu mundo[cite: 1]
    public float fuerzaDeFlotacion = 15f;
    public float resistenciaDelAgua = 3f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Si el objeto desciende por debajo del nivel del agua[cite: 1]
        if (transform.position.y < nivelDelAgua)
        {
            // Cuanto más se hunda, mayor fuerza ascendente recibe (Arquímedes)[cite: 1]
            float multiplicadorHundimiento = Mathf.Clamp01(nivelDelAgua - transform.position.y);

            rb.AddForce(Vector3.up * fuerzaDeFlotacion * multiplicadorHundimiento, ForceMode.Acceleration);

            // Fricción para frenar el movimiento vertical y angular dentro del agua[cite: 1]
            rb.linearDamping = resistenciaDelAgua;
            rb.angularDamping = resistenciaDelAgua;
        }
        else
        {
            // Fricción normal cuando está fuera del agua (en el aire)[cite: 1]
            rb.linearDamping = 0.05f;
            rb.angularDamping = 0.05f;
        }
    }
}