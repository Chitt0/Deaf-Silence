using System.Collections;
using UnityEngine;

public class AmbienteTormentaLoop : MonoBehaviour
{
    [Header("Referencias de Audio")]
    [SerializeField] private AudioSource audioLluvia;
    [SerializeField] private AudioSource audioTruenos;

    [Header("Ajustes de Tiempo")]
    [Tooltip("Tiempo de espera antes de que empiecen a sonar los truenos por primera vez para desfasar el loop de la lluvia.")]
    [SerializeField] private float retrasoInicialTruenos = 5f;

    [Header("Variación de Truenos")]
    [SerializeField] private bool variarPitchEnCadaLoop = true;
    [SerializeField] private float pitchMinimo = 0.92f;
    [SerializeField] private float pitchMaximo = 1.05f;

    void Start()
    {
        // 1. Iniciar la lluvia de inmediato
        if (audioLluvia != null)
        {
            audioLluvia.loop = true;
            audioLluvia.Play();
        }

        // 2. Iniciar la secuencia de truenos desfasada
        if (audioTruenos != null)
        {
            audioTruenos.loop = true;
            StartCoroutine(IniciarTruenosDesfasados());
        }
    }

    private IEnumerator IniciarTruenosDesfasados()
    {
        yield return new WaitForSeconds(retrasoInicialTruenos);
        audioTruenos.Play();

        // Corrutina opcional para cambiar sutilmente la velocidad/tono cada vez que se repite el clip
        if (variarPitchEnCadaLoop)
        {
            float duracionClip = audioTruenos.clip != null ? audioTruenos.clip.length : 40f;

            while (audioTruenos.isPlaying)
            {
                yield return new WaitForSeconds(duracionClip);
                // Cambia ligeramente el pitch en cada repetición para que las pausas y tonos cambien
                audioTruenos.pitch = Random.Range(pitchMinimo, pitchMaximo);
            }
        }
    }
}