using UnityEngine;

public class DisparadorEventoVentana : MonoBehaviour
{
    [SerializeField] private EntidadVentana entidad;

    void OnEnable()
    {
        GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        entidad.Activar();
        GetComponent<Collider>().enabled = false;
    }
}