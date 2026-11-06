using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        if (clickSound == null) return;

        // Crea un audio temporal en la posición de la cámara.
        // Así el sonido no se corta aunque el panel de la pregunta se desactive al hacer clic.
        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, volume);
    }
}