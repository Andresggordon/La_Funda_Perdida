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

        // 1. Caso: Es un Coleccionable / Trofeo
        if (datosDelTrofeo != null)
        {
            SaveManager saveManager = FindFirstObjectByType<SaveManager>();
            if (saveManager != null)
            {
                DatosPartida datos = saveManager.CargarPartida() ?? new DatosPartida();

                if (!datos.trofeosDesbloqueadosID.Contains(datosDelTrofeo.trophyID))
                {
                    datos.trofeosDesbloqueadosID.Add(datosDelTrofeo.trophyID);
                }

                if (idObjeto != null && !string.IsNullOrEmpty(idObjeto.idUnico))
                {
                    if (!datos.objetosDestruidosUID.Contains(idObjeto.idUnico))
                    {
                        datos.objetosDestruidosUID.Add(idObjeto.idUnico);
                    }
                    
                    if (inventarioJugador != null && !inventarioJugador.objetosDestruidosUID.Contains(idObjeto.idUnico))
                    {
                        inventarioJugador.objetosDestruidosUID.Add(idObjeto.idUnico);
                    }
                }

                saveManager.GuardarPartida(datos);
                Debug.Log("<color=yellow>[Coleccionable Desbloqueado]</color> " + datosDelTrofeo.trophyName);
            }

            Destroy(gameObject);
            return;
        }

        // 2. Caso: Es un Objeto común de Inventario
        if (datosDelObjeto != null && inventarioJugador != null)
        {
            inventarioJugador.AnadirObjeto(datosDelObjeto);

            if (idObjeto != null && !string.IsNullOrEmpty(idObjeto.idUnico))
            {
                if (!inventarioJugador.objetosDestruidosUID.Contains(idObjeto.idUnico))
                {
                    inventarioJugador.objetosDestruidosUID.Add(idObjeto.idUnico);
                }
            }

            Destroy(gameObject);
        }
    }
}