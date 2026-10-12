using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI clickTextComponent; // El texto que parpadea
    [SerializeField] private GameObject titleUI;                // El contenedor del título que se ocultará

    [Header("Efecto Parpadeo (Alpha)")]
    [SerializeField] private float blinkSpeed = 2.0f;
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 1.0f;

    [Header("Efecto Fade a Negro")]
    [SerializeField] private Image fadeImage;                   // La imagen negra para el fundido
    [SerializeField] private float fadeDuration = 1.0f;         // Duración del fundido

    [Header("Cámaras y Jugador")]
    [SerializeField] private GameObject menuCamera;             // Tu cámara del menú
    [SerializeField] private GameObject playerCamera;           // Tu cámara del jugador
    [SerializeField] private MonoBehaviour playerMovementScript;// El script de movimiento del personaje

    private AudioSource audioSource;
    private bool isStarted = false;

    void Start()
    {
        // El script busca automáticamente el Audio Source que pusiste en este mismo objeto
        audioSource = GetComponent<AudioSource>();

        // Aseguramos que la imagen del fade sea totalmente transparente al iniciar
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }

        // Al iniciar, congelamos al jugador y activamos la cámara del menú
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerCamera != null) playerCamera.SetActive(false);
        if (menuCamera != null) menuCamera.SetActive(true);
    }

    void Update()
    {
        if (!isStarted)
        {
            // 1. Hace parpadear el texto de "Haz click..."
            BlinkClickText();

            // 2. Detecta el clic izquierdo del ratón
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(FadeAndStartGame());
            }
        }
    }

    void BlinkClickText()
    {
        if (clickTextComponent != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * blinkSpeed, 1f));
            Color currentColor = clickTextComponent.color;
            currentColor.a = alpha;
            clickTextComponent.color = currentColor;
        }
    }

    IEnumerator FadeAndStartGame()
    {
        isStarted = true; // Bloquea nuevos clics

        // 3. Reproduce el sonido de impacto automáticamente al hacer clic
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        // --- FADE A NEGRO ---
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }
            yield return null;
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

        // Pequeña pausa de seguridad con la pantalla en negro
        yield return new WaitForSeconds(0.2f);

        // --- TRANSICIÓN (Con la pantalla en negro) ---
        if (titleUI != null) titleUI.SetActive(false);
        if (menuCamera != null) menuCamera.SetActive(false);
        if (playerCamera != null) playerCamera.SetActive(true);
        if (playerMovementScript != null) playerMovementScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // --- FADE OUT (DE NEGRO A TRANSPARENTE) ---
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }
            yield return null;
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }
}