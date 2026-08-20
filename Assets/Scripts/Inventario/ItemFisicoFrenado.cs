using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemFisicoFrenado : MonoBehaviour
{
    private Rigidbody rb;
    private bool haColisionado = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Solo ejecutamos esto en la primera colisión fuerte contra el terreno u otro objeto
        if (!haColisionado)
        {
            haColisionado = true;

            // 1. Multiplicamos la amortiguación al impactar para frenar la rotación en seco
            rb.linearDamping = 3f;
            rb.angularDamping = 5f;

            // 2. Si la velocidad de impacto es baja, forzamos al Rigidbody a dormir (pararse por completo)
            if (rb.linearVelocity.magnitude < 1.5f)
            {
                rb.Sleep();
            }
        }
    }
}