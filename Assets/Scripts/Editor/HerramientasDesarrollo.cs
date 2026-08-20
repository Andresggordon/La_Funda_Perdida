using UnityEngine;
using UnityEditor;
using System.IO;

public class HerramientasDesarrollo : MonoBehaviour
{
    // Esto creará un nuevo botón en la barra superior de Unity
    [MenuItem("La Funda Perdida/Borrar Datos de Guardado (WIPE)")]
    public static void BorrarPartida()
    {
        // 1. Borramos el archivo JSON de tu SaveManager
        string rutaArchivo = Application.persistentDataPath + "/datosGuardados.json";

        if (File.Exists(rutaArchivo))
        {
            File.Delete(rutaArchivo);
            Debug.Log("<color=red>[WIPE]</color> Archivo JSON de guardado ELIMINADO en: " + rutaArchivo);
        }
        else
        {
            Debug.Log("<color=yellow>[WIPE]</color> No se encontró ningún archivo JSON para borrar.");
        }

        // 2. Borramos los ajustes de audio, sensibilidad y FOV de PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("<color=red>[WIPE]</color> PlayerPrefs (Opciones) ELIMINADOS.");
    }
}