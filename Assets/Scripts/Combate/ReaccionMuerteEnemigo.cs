// ==========================================
// SCRIPT: ReaccionMuerteEnemigo.cs (Ejemplo)
// ==========================================
using UnityEngine;

[RequireComponent(typeof(SistemaSalud))]
public class ReaccionMuerteEnemigo : MonoBehaviour
{
    private SistemaSalud miSalud;

    private void Awake()
    {
        miSalud = GetComponent<SistemaSalud>();
    }

    private void OnEnable()
    {
        // Nos suscribimos al evento de muerte
        miSalud.AlMorir += EjecutarMuerte;
    }

    private void OnDisable()
    {
        // Nos desuscribimos por seguridad si el objeto se apaga
        miSalud.AlMorir -= EjecutarMuerte;
    }

    private void EjecutarMuerte()
    {
        Debug.Log(gameObject.name + " ha muerto. Soltando loot y activando ragdoll...");
        // Aquí iría tu lógica: soltar objetos (ItemData), activar física ragdoll, ganar experiencia, etc.
        Destroy(gameObject, 2f); // Destruye el cuerpo tras 2 segundos
    }
}