using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI clickTextComponent;

    [Header("Efecto Parpadeo (Alpha)")]
    [SerializeField] private float blinkSpeed = 2.0f;
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 1.0f;

    [Header("Efecto Fade a Negro")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float blackPauseDuration = 0.2f;

    [Header("Escena del juego")]
    [SerializeField] private string gameSceneName = "BlockoutMapa";

    private AudioSource audioSource;
    private bool isStarted = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetFadeAlpha(0f);
    }

    void Update()
    {
        if (isStarted) return;

        BlinkClickText();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(FadeAndStartGame());
        }
    }

    void BlinkClickText()
    {
        if (clickTextComponent == null) return;

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * blinkSpeed, 1f));
        Color currentColor = clickTextComponent.color;
        currentColor.a = alpha;
        clickTextComponent.color = currentColor;
    }

    IEnumerator FadeAndStartGame()
    {
        isStarted = true;

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            SetFadeAlpha(Mathf.Lerp(0f, 1f, timer / fadeDuration));
            yield return null;
        }
        SetFadeAlpha(1f);

        yield return new WaitForSeconds(blackPauseDuration);

        SceneManager.LoadScene(gameSceneName);
    }

    void SetFadeAlpha(float alpha)
    {
        if (fadeImage == null) return;

        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}