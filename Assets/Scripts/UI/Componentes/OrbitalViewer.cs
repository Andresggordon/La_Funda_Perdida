using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitalViewer : MonoBehaviour
{
    [Header("Ajustes de Rotación")]
    public float velocidadRaton = 0.2f;
    public float velocidadGamepad = 120f;
    public Transform targetToRotate;

    private void Update()
    {
        if (targetToRotate == null) return;

        float deltaX = 0f;
        float deltaY = 0f;

        // 1. Ratón (Manteniendo clic izquierdo)
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            deltaX += mouseDelta.x * velocidadRaton;
            deltaY += mouseDelta.y * velocidadRaton;
        }

        // 2. Mando PS4 (Stick Derecho / Acción Mirar)
        if (Gamepad.current != null)
        {
            Vector2 stickDelta = Gamepad.current.rightStick.ReadValue();
            deltaX += stickDelta.x * velocidadGamepad * Time.unscaledDeltaTime;
            deltaY += stickDelta.y * velocidadGamepad * Time.unscaledDeltaTime;
        }

        if (Mathf.Abs(deltaX) > 0.001f || Mathf.Abs(deltaY) > 0.001f)
        {
            targetToRotate.Rotate(Vector3.up, -deltaX, Space.World);
            targetToRotate.Rotate(Camera.main != null ? Camera.main.transform.right : Vector3.right, deltaY, Space.World);
        }
    }

    public void SetTarget(Transform nuevoTarget)
    {
        targetToRotate = nuevoTarget;
    }
}