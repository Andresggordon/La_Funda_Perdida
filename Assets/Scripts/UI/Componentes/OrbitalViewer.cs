using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitalViewer : MonoBehaviour
{
    [Header("Ajustes de Rotación")]
    public float velocidadRaton = 0.5f;
    public float velocidadGamepad = 150f;
    
    private Transform targetToRotate;

    private void Update()
    {
        if (targetToRotate == null) return;

        float deltaX = 0f;
        float deltaY = 0f;

        // Soporte Ratón (Clic izquierdo mantenido)
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            deltaX += mouseDelta.x * velocidadRaton;
            deltaY += mouseDelta.y * velocidadRaton;
        }

        // Soporte Mando PS4 (Stick Derecho)
        if (Gamepad.current != null)
        {
            Vector2 stickDelta = Gamepad.current.rightStick.ReadValue();
            deltaX += stickDelta.x * velocidadGamepad * Time.unscaledDeltaTime;
            deltaY += stickDelta.y * velocidadGamepad * Time.unscaledDeltaTime;
        }

        if (Mathf.Abs(deltaX) > 0.001f || Mathf.Abs(deltaY) > 0.001f)
        {
            targetToRotate.Rotate(Vector3.up, -deltaX, Space.World);
            targetToRotate.Rotate(Camera.main.transform.right, deltaY, Space.World);
        }
    }

    public void SetTarget(Transform nuevoTarget)
    {
        targetToRotate = nuevoTarget;
    }
}