using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class JugadorInteraccion : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    [SerializeField] private float distanciaInteraccion = 5f;
    [SerializeField] private LayerMask capaInteractuable;

    [Header("Referencias")]
    [SerializeField] private Camera camaraPrincipal;
    [SerializeField] private JugadorCamara jugadorCamara;
    [SerializeField] private Transform transformCara;

    [Header("UI (Solo Iconos)")]
    [SerializeField] private Image iconoInteraccionUI;
    [SerializeField] private Sprite imagenTecladoE;
    [SerializeField] private Sprite imagenMandoCuadrado;

    private InventoryManager miInventario;
    private ObjetoInteractuable objetoMirado;

    private void Start()
    {
        miInventario = GetComponent<InventoryManager>();
        OcultarUI();
    }

    private void Update()
    {
        DetectarObjeto();
        ComprobarInput();
    }

    private void DetectarObjeto()
    {
        if (camaraPrincipal == null || jugadorCamara == null || transformCara == null) return;

        Ray rayo;

        if (jugadorCamara.estadoCamara == 0)
        {
            rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        }
        else
        {
            rayo = new Ray(transformCara.position, transformCara.forward);
        }

        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, distanciaInteraccion, capaInteractuable))
        {
            if (impacto.collider.TryGetComponent(out ObjetoInteractuable interactuable))
            {
                if (interactuable != objetoMirado)
                {
                    if (objetoMirado != null)
                    {
                        objetoMirado.DesactivarResaltado();
                        OcultarUI();
                    }

                    objetoMirado = interactuable;
                    objetoMirado.ActivarResaltado();

                    if (objetoMirado.TryGetComponent(out ObjetoRecogible recogible))
                    {
                        MostrarIcono(); // Solo mostramos el botón
                    }
                }
            }
        }
        else
        {
            if (objetoMirado != null)
            {
                objetoMirado.DesactivarResaltado();
                objetoMirado = null;
                OcultarUI();
            }
        }
    }

    private void ComprobarInput()
    {
        bool botonPulsado = false;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            botonPulsado = true;
        }

        if (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            botonPulsado = true;
        }

        if (botonPulsado && objetoMirado != null)
        {
            if (objetoMirado.TryGetComponent(out ObjetoRecogible recogible))
            {
                recogible.Recoger(miInventario);
                objetoMirado = null;
                OcultarUI();
            }
        }
    }

    private void MostrarIcono()
    {
        if (iconoInteraccionUI != null)
        {
            if (Gamepad.current != null)
            {
                iconoInteraccionUI.sprite = imagenMandoCuadrado;
            }
            else
            {
                iconoInteraccionUI.sprite = imagenTecladoE;
            }

            iconoInteraccionUI.gameObject.SetActive(true);
        }
    }

    private void OcultarUI()
    {
        if (iconoInteraccionUI != null) iconoInteraccionUI.gameObject.SetActive(false);
    }
}