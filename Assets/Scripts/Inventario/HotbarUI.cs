using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Necesario para el nuevo Input System

public class HotbarUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private InventoryManager inventarioJugador;

    [Header("Configuración de Slots")]
    [SerializeField] private Image[] imagenesSlots; // Las imágenes donde se dibuja el icono de la seta
    [SerializeField] private GameObject[] marcosSeleccion; // (Opcional) Un borde o marco visual para saber qué slot está activo

    private int slotActivoIndex = 0; // Empieza seleccionando el primer slot (0)

    private void Update()
    {
        ComprobarInputsHotbar();
        ActualizarBarra();
    }

    private void ComprobarInputsHotbar()
    {
        // --- 1. TECLADO (Números 1, 2, 3, 4) ---
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame) slotActivoIndex = 0;
            if (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame) slotActivoIndex = 1;
            if (Keyboard.current.digit3Key.wasPressedThisFrame || Keyboard.current.numpad3Key.wasPressedThisFrame) slotActivoIndex = 2;
            if (Keyboard.current.digit4Key.wasPressedThisFrame || Keyboard.current.numpad4Key.wasPressedThisFrame) slotActivoIndex = 3;
        }

        // --- 2. MANDO (Cruceta / D-Pad Izquierda y Derecha) ---
        if (Gamepad.current != null)
        {
            // Si pulsamos la flecha derecha de la cruceta, avanzamos un slot
            if (Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                slotActivoIndex++;
                if (slotActivoIndex > 3) slotActivoIndex = 0; // Si pasa del último, vuelve al primero (bucle)
            }

            // Si pulsamos la flecha izquierda de la cruceta, retrocedemos un slot
            if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                slotActivoIndex--;
                if (slotActivoIndex < 0) slotActivoIndex = 3; // Si baja del primero, va al último (bucle)
            }
        }
    }

    private void ActualizarBarra()
    {
        if (inventarioJugador == null) return;

        // Recorremos los 4 slots de la pantalla
        for (int i = 0; i < imagenesSlots.Length; i++)
        {
            // A. Dibujar los iconos de los objetos que tenemos en los bolsillos
            if (i < inventarioJugador.objetosActuales.Count)
            {
                ItemData objetoEnSlot = inventarioJugador.objetosActuales[i];
                if (objetoEnSlot != null && objetoEnSlot.icono != null)
                {
                    imagenesSlots[i].sprite = objetoEnSlot.icono;
                    Color color = imagenesSlots[i].color;
                    color.a = 1f;
                    imagenesSlots[i].color = color;
                }
            }
            else
            {
                imagenesSlots[i].sprite = null;
                Color color = imagenesSlots[i].color;
                color.a = 0f;
                imagenesSlots[i].color = color;
            }

            // B. Activar o desactivar el marco visual del slot seleccionado
            if (marcosSeleccion != null && i < marcosSeleccion.Length && marcosSeleccion[i] != null)
            {
                if (i == slotActivoIndex)
                {
                    marcosSeleccion[i].SetActive(true); // Muestra el marco en el slot activo
                }
                else
                {
                    marcosSeleccion[i].SetActive(false); // Oculta el marco en los demás
                }
            }
        }
    }
}