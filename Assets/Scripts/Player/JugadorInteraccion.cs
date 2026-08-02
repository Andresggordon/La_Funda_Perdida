using UnityEngine;

public class JugadorInteraccion : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    [SerializeField] private float distanciaInteraccion = 3f;
    [SerializeField] private LayerMask capaInteractuable;

    [Header("Referencias")]
    [SerializeField] private Transform transformOjos; // Tu SM_Chibi_Eye
    [SerializeField] private Camera camaraPrincipal;  // La cámara principal
    [SerializeField] private JugadorCamara jugadorCamara; // Referencia a tu script de cámara

    private ObjetoInteractuable objetoMirado;

    private void Update()
    {
        DetectarObjeto();
    }

    private void DetectarObjeto()
    {
        if (transformOjos == null || camaraPrincipal == null || jugadorCamara == null) return;

        // Copiamos la rotación de la cámara
        transformOjos.rotation = camaraPrincipal.transform.rotation;

        Vector3 origenRayo = transformOjos.position;
        Vector3 direccionRayo = transformOjos.forward;

        // ACCEDEMOS A TU ESTADO DE CÁMARA:
        // Como no tenemos el estadoCamara público directamente, podemos saber si estamos en vista frontal (estado 2) 
        // mirando si la rotación local en Y de la cámara tiene el giro de 180 grados (aprox), 
        // O podemos hacer que estadoCamara sea público en el otro script. 
        // Pero una forma rapidísima y sin tocar el otro script es comprobar si la cámara está mirando hacia el personaje:

        // Si la cámara está delante del personaje (en posición Z positiva como en tu posVistaFrontal), 
        // invertimos el rayo para que salga hacia el frente del personaje:
        if (camaraPrincipal.transform.localPosition.z > 0f)
        {
            direccionRayo = -transformOjos.forward; // Invertimos el rayo en segunda persona
        }

        RaycastHit impacto;

        if (Physics.Raycast(origenRayo, direccionRayo, out impacto, distanciaInteraccion, capaInteractuable))
        {
            if (impacto.collider.TryGetComponent(out ObjetoInteractuable interactuable))
            {
                if (interactuable != objetoMirado)
                {
                    if (objetoMirado != null)
                    {
                        objetoMirado.DesactivarResaltado();
                    }

                    objetoMirado = interactuable;
                    objetoMirado.ActivarResaltado();
                }
            }
        }
        else
        {
            if (objetoMirado != null)
            {
                objetoMirado.DesactivarResaltado();
                objetoMirado = null;
            }
        }
    }
}