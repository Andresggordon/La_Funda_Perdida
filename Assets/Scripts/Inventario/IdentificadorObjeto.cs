using UnityEngine;

public class IdentificadorObjeto : MonoBehaviour
{
    // Este ID debe ser único para cada objeto recolectable del mundo
    public string idUnico;

    [ContextMenu("Generar ID Automático")]
    private void GenerarID()
    {
        // Esto te crea un código aleatorio único con un solo clic en el Inspector
        idUnico = System.Guid.NewGuid().ToString();
    }
}