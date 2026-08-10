using UnityEngine;

public class ObjetoRecogible : MonoBehaviour
{
    [Header("¿Qué objeto es este?")]
    public ItemData datosDelObjeto;

    private void Start()
    {
        // Consultamos directamente al guardado al nacer, sin esperar al inventario
        SaveManager saveManager = FindFirstObjectByType<SaveManager>();
        IdentificadorObjeto idObjeto = GetComponent<IdentificadorObjeto>();

        if (saveManager != null && idObjeto != null && !string.IsNullOrEmpty(idObjeto.idUnico))
        {
            DatosPartida datos = saveManager.CargarPartida();
            if (datos != null && datos.objetosDestruidosUID != null)
            {
                if (datos.objetosDestruidosUID.Contains(idObjeto.idUnico))
                {
                    // Si su ID está en la lista guardada, se borra al instante
                    Destroy(gameObject);
                }
            }
        }
    }

    public void Recoger(InventoryManager inventarioJugador)
    {
        if (inventarioJugador != null)
        {
            // 1. Lo metemos en el inventario
            inventarioJugador.AnadirObjeto(datosDelObjeto);

            // 2. Apuntamos nuestro ID en la lista del inventario
            IdentificadorObjeto idObjeto = GetComponent<IdentificadorObjeto>();
            if (idObjeto != null && !string.IsNullOrEmpty(idObjeto.idUnico))
            {
                if (!inventarioJugador.objetosDestruidosUID.Contains(idObjeto.idUnico))
                {
                    inventarioJugador.objetosDestruidosUID.Add(idObjeto.idUnico);
                }
            }

            // 3. Desaparece del suelo
            Destroy(gameObject);
        }
    }
}