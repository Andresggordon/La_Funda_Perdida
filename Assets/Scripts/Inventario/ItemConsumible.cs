using UnityEngine;

// Fíjate en cómo creamos un menú diferente en Unity para este tipo de objeto específico
[CreateAssetMenu(fileName = "NuevoConsumible", menuName = "Inventario/Item Consumible")]
public class ItemConsumible : ItemData
{
    [Header("Efectos del Consumible")]
    public int corazonesARecuperar = 1;
    public AudioClip sonidoAlConsumir;

    // Sobrescribimos la acción "Usar"
    public override bool Usar(GameObject jugador)
    {
        Debug.Log("Paula ha consumido " + nombreMostrado + " y recuperó " + corazonesARecuperar + " corazones.");

        // Aquí conectaremos más adelante tu script de Vida/Corazones de Paula
        // jugador.GetComponent<SaludJugador>().Curar(corazonesARecuperar);

        if (sonidoAlConsumir != null)
        {
            AudioSource.PlayClipAtPoint(sonidoAlConsumir, jugador.transform.position);
        }

        // Devolvemos true para decirle al inventario que este objeto debe borrarse (se gastó)
        return true;
    }
}