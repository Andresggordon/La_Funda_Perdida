using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnfocarMandoUI : MonoBehaviour
{
    [Header("Elemento a seleccionar al abrir este panel")]
    public GameObject primerElemento;

    private void OnEnable()
    {
        if (primerElemento != null)
        {
            StartCoroutine(SeleccionarConRetraso());
        }
    }

    private IEnumerator SeleccionarConRetraso()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(primerElemento);
    }
}