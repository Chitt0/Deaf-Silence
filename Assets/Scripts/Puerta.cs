using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField] private Transform destinoEntrada;
    [SerializeField] private bool cuentaComoVuelta = false;
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractorJugador interactor = other.GetComponent<InteractorJugador>();
            if (interactor != null)
            {
                interactor.RegistrarPuertaCercana(this);
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
                interactor.QuitarPuertaCercana(this);
            }
        }
    }

    public Vector3 ObtenerPosicionDestino()
    {
        return destinoEntrada.position;
    }

    public Quaternion ObtenerRotacionDestino()
    {
        return destinoEntrada.rotation;
    }

    public bool CuentaComoVuelta()
    {
        return cuentaComoVuelta;
    }

    public void ReproducirSonido()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}