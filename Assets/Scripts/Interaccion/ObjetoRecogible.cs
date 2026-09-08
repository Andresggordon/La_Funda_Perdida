using UnityEngine;

public class ObjetoRecogible : MonoBehaviour, IInteractuable
{
    [Header("Tipo de Objeto")]
    [Tooltip("Asigna aquí el ScriptableObject si es un objeto de inventario normal.")]
    public ItemData datosDelObjeto;

    [Tooltip("Asigna aquí el ScriptableObject si este objeto es un Trofeo de Galería.")]
    public TrofeoData datosDelTrofeo;

    private void Start()
    {
        SaveManager saveManager = FindFirstObjectByType<SaveManager>();
        IdentificadorObjeto idObjeto = GetComponent<IdentificadorObjeto>();

        if (saveManager != null && idObjeto != null && !string.IsNullOrEmpty(idObjeto.idUnico))
        {
            DatosPartida datos = saveManager.CargarPartida();
            if (datos != null && datos.objetosDestruidosUID != null)
            {
                if (datos.objetosDestruidosUID.Contains(idObjeto.idUnico))
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void EjecutarInteraccion(GameObject jugador)
    {
        InventoryManager inventarioJugador = jugador.GetComponent<InventoryManager>();
        IdentificadorObjeto idObjeto = GetComponent<IdentificadorObjeto>();

        bool objetoProcesado = false;

        // --- 1. LÓGICA DE LA GALERÍA DE TROFEOS ---
        if (datosDelTrofeo != null)
        {
            SaveManager saveManager = FindFirstObjectByType<SaveManager>();
            if (saveManager != null)
            {
                DatosPartida datos = saveManager.CargarPartida() ?? new DatosPartida();

                if (!datos.trofeosDesbloqueadosID.Contains(datosDelTrofeo.trophyID))
                {
                    datos.trofeosDesbloqueadosID.Add(datosDelTrofeo.trophyID);
                    saveManager.GuardarPartida(datos); // Guardamos el logro en el archivo JSON
                    Debug.Log("<color=yellow>[Coleccionable Desbloqueado en Galería]</color> " + datosDelTrofeo.trophyName);
                }
                objetoProcesado = true;
            }
        }

        // --- 2. LÓGICA DEL INVENTARIO Y HOTBAR ---
        if (datosDelObjeto != null && inventarioJugador != null)
        {
            inventarioJugador.AnadirObjeto(datosDelObjeto);
            Debug.Log("<color=green>[Objeto Añadido a la Mochila]</color> " + datosDelObjeto.nombreMostrado);
            objetoProcesado = true;
        }

        // --- 3. DESTRUCCIÓN Y PERSISTENCIA EN EL MUNDO ---
        if (objetoProcesado)
        {
            if (idObjeto != null && !string.IsNullOrEmpty(idObjeto.idUnico))
            {
                if (inventarioJugador != null && !inventarioJugador.objetosDestruidosUID.Contains(idObjeto.idUnico))
                {
                    inventarioJugador.objetosDestruidosUID.Add(idObjeto.idUnico);
                }
            }

            Destroy(gameObject); // Desaparece del suelo
        }
    }
}