using UnityEngine;

public class JugadorInteraccion : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    [SerializeField] private float distanciaInteraccion = 5f;
    [SerializeField] private LayerMask capaInteractuable;

    [Header("Referencias")]
    [SerializeField] private Camera camaraPrincipal;
    [SerializeField] private JugadorCamara jugadorCamara; // Para saber en qué perspectiva estamos
    [SerializeField] private Transform transformCara; // Arrastra aquí el objeto de los ojos/cara (SM_Chibi_Eye)

    private ObjetoInteractuable objetoMirado;

    private void Update()
    {
        DetectarObjeto();
    }

    private void DetectarObjeto()
    {
        // Si falta alguna referencia, no hacemos nada para evitar errores
        if (camaraPrincipal == null || jugadorCamara == null || transformCara == null) return;

        Ray rayo;

        // Comprobamos la cámara actual usando tu variable estadoCamara
        if (jugadorCamara.estadoCamara == 0)
        {
            // ESTADO 0: PRIMERA PERSONA
            // El rayo sale de la cámara, apuntando al centro exacto de la pantalla (donde está tu mirilla)
            rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        }
        else
        {
            // ESTADOS 1 y 2: TERCERA Y SEGUNDA PERSONA
            // El rayo sale de la cara del personaje (por delante) y va hacia donde mira su cuerpo/cabeza
            rayo = new Ray(transformCara.position, transformCara.forward);
        }

        RaycastHit impacto;

        // Lanzamos el rayo elegido
        if (Physics.Raycast(rayo, out impacto, distanciaInteraccion, capaInteractuable))
        {
            if (impacto.collider.TryGetComponent(out ObjetoInteractuable interactuable))
            {
                if (interactuable != objetoMirado)
                {
                    if (objetoMirado != null) objetoMirado.DesactivarResaltado();

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