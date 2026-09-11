using System.Collections;
using UnityEngine;

public class LuzParpadeante : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Light luz;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipZumbido;

    [Header("Ritmo de episodios de parpadeo")]
    [SerializeField] private float tiempoMinEntreEpisodios = 2f;
    [SerializeField] private float tiempoMaxEntreEpisodios = 5f;
    [SerializeField] private int parpadeosMinPorEpisodio = 1;
    [SerializeField] private int parpadeosMaxPorEpisodio = 4;

    [Header("Duración de cada parpadeo individual")]
    [SerializeField] private float duracionMinParpadeo = 0.05f;
    [SerializeField] private float duracionMaxParpadeo = 0.2f;

    [Header("Variación de intensidad")]
    [SerializeField] private float intensidadMin = 150f;
    [SerializeField] private float intensidadMax = 400f;
    [SerializeField] private float tiempoMinEntreCambiosIntensidad = 0.05f;
    [SerializeField] private float tiempoMaxEntreCambiosIntensidad = 0.3f;

    void Start()
    {
        if (audioSource != null && clipZumbido != null)
        {
            audioSource.clip = clipZumbido;
            audioSource.loop = true;
            audioSource.Play();
        }

        StartCoroutine(CicloDeParpadeo());
        StartCoroutine(CicloDeIntensidad());
    }

    private IEnumerator CicloDeParpadeo()
    {
        while (true)
        {
            float espera = Random.Range(tiempoMinEntreEpisodios, tiempoMaxEntreEpisodios);
            yield return new WaitForSeconds(espera);

            int cantidadParpadeos = Random.Range(parpadeosMinPorEpisodio, parpadeosMaxPorEpisodio + 1);

            for (int i = 0; i < cantidadParpadeos; i++)
            {
                luz.enabled = false;
                yield return new WaitForSeconds(Random.Range(duracionMinParpadeo, duracionMaxParpadeo));
                luz.enabled = true;
                yield return new WaitForSeconds(Random.Range(duracionMinParpadeo, duracionMaxParpadeo));
            }
        }
    }

    private IEnumerator CicloDeIntensidad()
    {
        while (true)
        {
            luz.intensity = Random.Range(intensidadMin, intensidadMax);
            yield return new WaitForSeconds(Random.Range(tiempoMinEntreCambiosIntensidad, tiempoMaxEntreCambiosIntensidad));
        }
    }
}