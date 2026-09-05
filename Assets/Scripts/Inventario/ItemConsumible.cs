using UnityEngine;

// Fíjate en cómo creamos un menú diferente en Unity para este tipo de objeto específico
[CreateAssetMenu(fileName = "NuevoConsumible", menuName = "Inventario/Item Consumible")]
public class ItemConsumible : ItemData
{
    [Header("Efectos del Consumible")]
    public int corazonesARecuperar = 1;
    public AudioClip sonidoAlConsumir;

    public override bool Usar(GameObject jugador)
    {
        if (jugador == null || !jugador.TryGetComponent(out SistemaSalud salud))
            return false;

        salud.Curar(corazonesARecuperar);

        if (sonidoAlConsumir != null)
            AudioSource.PlayClipAtPoint(sonidoAlConsumir, jugador.transform.position);

        return true;
    }
}