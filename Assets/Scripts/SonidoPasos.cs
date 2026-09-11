using UnityEngine;

public class SonidoPasos : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipConcreto;
    [SerializeField] private AudioClip clipMadera;
    [SerializeField] private AudioClip clipMaderaCorrer;

    [Header("Pitch (solo concreto)")]
    [SerializeField] private float pitchCaminar = 1f;
    [SerializeField] private float pitchCorrer = 1.3f;

    [Header("Detección de piso")]
    [SerializeField] private float distanciaRaycast = 1.5f;

    private Vector2 inputMovimientoRef;
    private bool corriendoRef;
    private bool moviendoseAnterior;
    private string pisoAnterior = "";
    private bool corriendoAnterior;

    void Update()
    {
        bool moviendose = inputMovimientoRef.magnitude > 0.1f;

        if (!moviendose)
        {
            if (moviendoseAnterior)
            {
                audioSource.Stop();
            }
            moviendoseAnterior = false;
            return;
        }

        string pisoActual = DetectarPiso();

        bool cambioDeEstado = !moviendoseAnterior
            || pisoActual != pisoAnterior
            || corriendoRef != corriendoAnterior;

        if (cambioDeEstado)
        {
            IniciarPaso(pisoActual);
            pisoAnterior = pisoActual;
            corriendoAnterior = corriendoRef;
            moviendoseAnterior = true;
        }
    }

    public void ActualizarEstado(Vector2 inputMovimiento, bool corriendo)
    {
        inputMovimientoRef = inputMovimiento;
        corriendoRef = corriendo;
    }

    private void IniciarPaso(string tagPiso)
    {
        if (tagPiso == "PisoMadera")
        {
            audioSource.pitch = 1f;
            audioSource.clip = corriendoRef ? clipMaderaCorrer : clipMadera;
        }
        else
        {
            audioSource.pitch = corriendoRef ? pitchCorrer : pitchCaminar;
            audioSource.clip = clipConcreto;
        }

        audioSource.loop = true;
        audioSource.Play();
    }

    private string DetectarPiso()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanciaRaycast))
        {
            return hit.collider.tag;
        }
        return "";
    }
}