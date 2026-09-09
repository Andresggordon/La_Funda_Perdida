using UnityEngine;

public class AnimEventRelay : MonoBehaviour
{
    [Header("Conexión con el Cerebro")]
    [Tooltip("Arrastra aquí el objeto raíz de Paula que contiene el GestorEquipamiento")]
    public GestorEquipamiento gestorEquipamiento;

    // Esta función SÍ será detectada por el Animator
    public void Evento_LanzarObjeto()
    {
        if (gestorEquipamiento != null)
        {
            gestorEquipamiento.EjecutarDisparoFisico();
        }
    }
}