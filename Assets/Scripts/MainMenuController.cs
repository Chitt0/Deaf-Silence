using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Inicial")]
    [SerializeField] private TextMeshProUGUI clickTextComponent;
    [SerializeField] private GameObject titleUI;

    [Header("Efecto Parpadeo (Alpha)")]
    [SerializeField] private float blinkSpeed = 2.0f;
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 1.0f;

    [Header("Preguntas SÍ / NO")]
    [SerializeField] private GameObject panelPreguntas;
    [SerializeField] private TextMeshProUGUI textoPregunta;
    [SerializeField] private Button botonSi;
    [SerializeField] private Button botonNo;
    [SerializeField]
    private string[] preguntas = new string[]
    {
        "¿Le tienes miedo a la oscuridad?",
        "¿Alguna vez te has sentido observado?",
        "¿Crees en los fantasmas?",
        "¿Eres sordo/a?"
    };
    [Tooltip("Si está marcado, responder SÍ a la última pregunta desactiva el audio. Desmárcalo si la redactas como '¿Quieres jugar con sonido?'")]
    [SerializeField] private bool siDesactivaAudio = true;

    [Header("Efecto Fade a Negro")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float blackPauseDuration = 0.2f;

    [Header("Escena del juego")]
    [SerializeField] private string gameSceneName = "BlockoutMapa";

    private AudioSource audioSource;
    private bool clickInicialHecho = false;
    private bool terminado = false;
    private int indicePregunta = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetFadeAlpha(0f);

        if (panelPreguntas != null) panelPreguntas.SetActive(false);

        if (botonSi != null) botonSi.onClick.AddListener(() => Responder(true));
        if (botonNo != null) botonNo.onClick.AddListener(() => Responder(false));
    }

    void Update()
    {
        if (clickInicialHecho) return;

        BlinkClickText();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            IniciarPreguntas();
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

    void IniciarPreguntas()
    {
        clickInicialHecho = true;

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        if (clickTextComponent != null) clickTextComponent.gameObject.SetActive(false);
        if (titleUI != null) titleUI.SetActive(false);

        indicePregunta = 0;
        panelPreguntas.SetActive(true);
        MostrarPregunta();
    }

    void MostrarPregunta()
    {
        textoPregunta.text = preguntas[indicePregunta];
    }

    void Responder(bool respuestaSi)
    {
        if (terminado) return;

        bool esUltima = indicePregunta == preguntas.Length - 1;

        if (esUltima)
        {
            bool audioActivado = respuestaSi ? !siDesactivaAudio : siDesactivaAudio;

            ConfiguracionJuego.audioActivado = audioActivado;
            AudioListener.volume = audioActivado ? 1f : 0f;

            terminado = true;
            panelPreguntas.SetActive(false);
            StartCoroutine(FadeAndStartGame());
        }
        else
        {
            indicePregunta++;
            MostrarPregunta();
        }
    }

    IEnumerator FadeAndStartGame()
    {
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