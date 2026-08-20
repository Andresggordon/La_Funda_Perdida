using UnityEngine;
using System.IO; // ¡Súper importante! Nos permite crear y leer archivos en el ordenador.

public class SaveManager : MonoBehaviour
{
    private string rutaArchivo;

    private void Awake()
    {
        // Application.persistentDataPath es una carpeta oculta y segura que Unity asigna a tu juego
        // (En Windows suele estar en AppData/LocalLow/TuCompañia/TuJuego)
        rutaArchivo = Application.persistentDataPath + "/datosGuardados.json";
    }

    // --- 1. GUARDAR PARTIDA ---
    public void GuardarPartida(DatosPartida datosA_Guardar)
    {
        // Convertimos la "caja de mudanzas" en un texto con formato JSON
        // El 'true' al final es para que el texto se formatee con saltos de línea y sea fácil de leer
        string contenidoJSON = JsonUtility.ToJson(datosA_Guardar, true);

        // Escribimos ese texto en el archivo de nuestro ordenador
        File.WriteAllText(rutaArchivo, contenidoJSON);

        Debug.Log("Partida guardada con éxito en: " + rutaArchivo);
    }

    // --- 2. CARGAR PARTIDA ---
    public DatosPartida CargarPartida()
    {
        if (ExistePartida())
        {
            // Leemos todo el texto del archivo
            string contenidoJSON = File.ReadAllText(rutaArchivo);

            // Convertimos ese texto JSON de vuelta a nuestra clase DatosPartida
            DatosPartida datosCargados = JsonUtility.FromJson<DatosPartida>(contenidoJSON);

            Debug.Log("Partida cargada con éxito.");
            return datosCargados;
        }
        else
        {
            Debug.LogWarning("Intentaste cargar, pero no hay ningún archivo de guardado.");
            return null; // Devolvemos "nada" si no hay archivo
        }
    }

    // --- 3. COMPROBAR SI HAY PARTIDA (Para el Menú Principal) ---
    public bool ExistePartida()
    {
        return File.Exists(rutaArchivo);
    }

    // --- 4. BORRAR PARTIDA (Para reiniciar desde cero) ---
    public void BorrarPartida()
    {
        if (ExistePartida())
        {
            File.Delete(rutaArchivo);
            Debug.Log("Archivo de guardado eliminado correctamente.");
        }
    }
}