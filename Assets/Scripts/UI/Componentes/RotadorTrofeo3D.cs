using UnityEngine;
using UnityEngine.InputSystem; // <-- VITAL: Usamos el nuevo sistema de Inputs

public class RotadorTrofeo3D : MonoBehaviour
{
    private Transform target;
    
    [Header("Ajustes de Velocidad")]
    public float velocidadRaton = 0.2f;
    public float velocidadMando = 150f;

    private void Update()
    {
        if (target == null) return;

        Vector2 inputDelta = Vector2.zero;

        // 1. LECTURA DE RATÓN (Solo si hacemos clic izquierdo)
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            // ReadValue de delta ya nos da la diferencia en píxeles
            inputDelta = Mouse.current.delta.ReadValue() * velocidadRaton;
        }
        // 2. LECTURA DE MANDO PS4/XBOX (Con el Joystick Derecho)
        else if (Gamepad.current != null)
        {
            // El stick nos da un valor de -1 a 1, así que multiplicamos por Time.deltaTime
            inputDelta = Gamepad.current.rightStick.ReadValue() * velocidadMando * Time.deltaTime;
        }

        // 3. APLICAR ROTACIÓN
        if (inputDelta != Vector2.zero)
        {
            target.Rotate(Vector3.up, -inputDelta.x, Space.World);
            target.Rotate(Vector3.right, inputDelta.y, Space.World);
        }
    }
    
    public void SetTarget(Transform nuevoTarget)
    {
        target = nuevoTarget;
    }
}