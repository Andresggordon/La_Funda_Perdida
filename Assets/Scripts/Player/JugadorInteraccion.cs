using UnityEngine;

public class JugadorInteraccion : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    [SerializeField] private float distanciaInteraccion = 3f;
    [SerializeField] private LayerMask capaInteractuable; // Filtro para ignorar paredes y suelo

    [Header("Referencias")]
    [SerializeField] private Camera camaraPrincipal;

    private ObjetoInteractuable objetoMirado;

    private void Update()
    {
        // Lo ponemos en el Update para que compruebe constantemente qué estamos mirando
        DetectarObjeto();
    }

    private void DetectarObjeto()
    {
        Ray rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, distanciaInteraccion, capaInteractuable))
        {
            // OPTIMIZACIÓN PASO 1: Usamos TryGetComponent para evitar generar basura en memoria
            // y comprobar/asignar en un solo paso.
            if (impacto.collider.TryGetComponent(out ObjetoInteractuable interactuable))
            {
                // Si el objeto tiene el script y NO es el mismo que ya estábamos mirando...
                if (interactuable != objetoMirado)
                {
                    // Si ya teníamos otro objeto mirado de antes, le quitamos el resaltado
                    if (objetoMirado != null)
                    {
                        objetoMirado.DesactivarResaltado();
                    }

                    // Guardamos el nuevo objeto como el actual y lo resaltamos
                    objetoMirado = interactuable;
                    objetoMirado.ActivarResaltado();
                }
            }
        }
        else
        {
            // Si el rayo choca con una pared o mira al aire, y teníamos un objeto seleccionado...
            if (objetoMirado != null)
            {
                // Le quitamos el color y vaciamos la variable
                objetoMirado.DesactivarResaltado();
                objetoMirado = null;
            }
        }
    }
}