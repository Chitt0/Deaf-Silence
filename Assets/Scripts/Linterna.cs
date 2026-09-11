using UnityEngine;

public class Linterna : MonoBehaviour
{
    [SerializeField] private Light luzLinterna;
    [SerializeField] private Collider colliderPickup;

    private bool encendida = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractorJugador interactor = other.GetComponent<InteractorJugador>();
            if (interactor != null)
            {
                interactor.RegistrarLinternaCercana(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractorJugador interactor = other.GetComponent<InteractorJugador>();
            if (interactor != null)
            {
                interactor.QuitarLinternaCercana(this);
            }
        }
    }

    public void Recoger(Transform puntoMano)
    {
        colliderPickup.enabled = false;

        transform.SetParent(puntoMano);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        CambiarLayerRecursivo(gameObject, LayerMask.NameToLayer("ObjetoMano"));
    }

    public void AlternarEncendido()
    {
        encendida = !encendida;
        luzLinterna.enabled = encendida;
    }

    private void CambiarLayerRecursivo(GameObject objeto, int layer)
    {
        objeto.layer = layer;
        foreach (Transform hijo in objeto.transform)
        {
            CambiarLayerRecursivo(hijo.gameObject, layer);
        }
    }
}