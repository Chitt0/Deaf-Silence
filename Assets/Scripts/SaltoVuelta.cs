using UnityEngine;
using UnityEngine.InputSystem;

public class SaltoVuelta : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference accionSaltoVuelta;

    private void OnEnable()
    {
        if (accionSaltoVuelta != null)
        {
            accionSaltoVuelta.action.Enable();
            accionSaltoVuelta.action.performed += OnSaltoVuelta;
        }
    }

    private void OnDisable()
    {
        if (accionSaltoVuelta != null)
        {
            accionSaltoVuelta.action.performed -= OnSaltoVuelta;
            accionSaltoVuelta.action.Disable();
        }
    }

    private void OnSaltoVuelta(InputAction.CallbackContext context)
    {
        if (ContadorVueltas.Instancia != null)
        {
            ContadorVueltas.Instancia.SumarVuelta();
        }
        else
        {
            Debug.LogWarning("SaltoVuelta: no se encontró una instancia de ContadorVueltas en la escena.");
        }
    }
}