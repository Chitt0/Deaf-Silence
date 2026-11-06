using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SubtitleTrigger : MonoBehaviour
{
    [SerializeField] private SubtitleLine[] lines;
    [SerializeField] private bool playOnlyOnce = true;

    private bool hasPlayed = false;

    private void Reset()
    {
        // Se configura solo como trigger al añadir el script
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (playOnlyOnce && hasPlayed) return;

        hasPlayed = true;
        SubtitleManager.Instance.ShowSequence(lines);
    }
}