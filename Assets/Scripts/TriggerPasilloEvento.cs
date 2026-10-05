using UnityEngine;

public class TriggerPasilloEvento : MonoBehaviour
{
    [SerializeField] private EntidadJSPasillo entidad;

    private Collider zona;

    private void Awake()
    {
        zona = GetComponent<Collider>();
    }

    // Se ejecuta cada vez que el gestor activa el contenedor
    private void OnEnable()
    {
        zona.enabled = true;

        if (entidad != null)
        {
            entidad.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        zona.enabled = false;

        if (entidad != null)
        {
            entidad.IniciarCarrera();
        }
    }
}