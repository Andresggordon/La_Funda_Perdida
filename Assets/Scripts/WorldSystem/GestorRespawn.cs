// ==========================================
// SCRIPT: GestorRespawn.cs
// ==========================================
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorRespawn : MonoBehaviour
{
    [Header("Sistemas")]
    public SistemaSalud saludJugador;
    public SaveManager saveManager;

    [Header("Punto de Aparición")]
    public Transform puntoCama;

    private void OnEnable()
    {
        if (saludJugador != null)
        {
            saludJugador.AlMorir += IniciarRespawn;
        }
    }

    private void OnDisable()
    {
        if (saludJugador != null)
        {
            saludJugador.AlMorir -= IniciarRespawn;
        }
    }

    private void IniciarRespawn()
    {
        // Arrancamos una corrutina para poder hacer una pausa de 2 segundos
        StartCoroutine(RutinaRespawn());
    }

    private IEnumerator RutinaRespawn()
    {
        Debug.Log("Paula ha muerto. Reapareciendo en su cama en 2 segundos...");

        // 1. Pausa dramática antes de recargar
        yield return new WaitForSeconds(2f);

        if (saveManager != null && saveManager.ExistePartida())
        {
            // 2. Cargamos el estado del último guardado
            DatosPartida datos = saveManager.CargarPartida();

            if (datos != null && puntoCama != null)
            {
                // 3. Modificamos SÓLO la posición para que despierte en la cama
                datos.posicionJugador = puntoCama.position;

                // 4. Guardamos los datos actualizados silenciosamente
                saveManager.GuardarPartida(datos);
            }
        }

        // 5. Recargamos la escena actual para restaurar el mundo y devolverle la vida a Paula
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}