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

    [Header("Audio del clic")]
    [Tooltip("Arrastra aquí el objeto que tiene el AudioSource con el sonido. Si lo dejas vacío, busca uno en este mismo objeto.")]
    [SerializeField] private AudioSource audioSource;

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

    private bool clickInicialHecho = false;
    private bool terminado = false;
    private bool puedeResponder = false;
    private int indicePregunta = 0;

    void Start()
    {
        // Si no arrastraste el AudioSource, intenta buscarlo en este mismo objeto
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        // Por si una partida anterior dejó el volumen global en 0
        AudioListener.volume = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // La imagen de fade no debe bloquear los clics de los botones
        if (fadeImage != null) fadeImage.raycastTarget = false;
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

        // 1) Sonido
        if (audioSource == null)
        {
            Debug.LogWarning("MainMenuController: no hay AudioSource asignado, no sonará el efecto del clic.");
        }
        else if (audioSource.clip == null)
        {
            Debug.LogWarning("MainMenuController: el AudioSource no tiene AudioClip asignado.");
        }
        else
        {
            audioSource.Play();
        }

        // 2) Fade a negro -> las preguntas aparecen sobre el negro
        StartCoroutine(TransicionAPreguntas());
    }

    IEnumerator TransicionAPreguntas()
    {
        // Oscurece la pantalla y la deja en negro
        yield return Fade(0f, 1f, fadeDuration);

        // Con la pantalla en negro, cambiamos la UI sin que se note
        if (clickTextComponent != null) clickTextComponent.gameObject.SetActive(false);
        if (titleUI != null) titleUI.SetActive(false);

        yield return new WaitForSecondsRealtime(blackPauseDuration);

        // Las preguntas aparecen sobre el fondo negro (el fade NO se quita)
        indicePregunta = 0;
        panelPreguntas.SetActive(true);
        MostrarPregunta();

        puedeResponder = true;
    }

    void MostrarPregunta()
    {
        textoPregunta.text = preguntas[indicePregunta];
    }

    void Responder(bool respuestaSi)
    {
        if (terminado || !puedeResponder) return;

        bool esUltima = indicePregunta == preguntas.Length - 1;

        if (esUltima)
        {
            bool audioActivado = respuestaSi ? !siDesactivaAudio : siDesactivaAudio;

            ConfiguracionJuego.audioActivado = audioActivado;
            AudioListener.volume = audioActivado ? 1f : 0f;

            terminado = true;
            panelPreguntas.SetActive(false);
            StartCoroutine(CargarJuego());
        }
        else
        {
            indicePregunta++;
            MostrarPregunta();
        }
    }

    IEnumerator CargarJuego()
    {
        // La pantalla ya está en negro desde antes; solo una pausa breve y cargamos
        SetFadeAlpha(1f);
        yield return new WaitForSecondsRealtime(blackPauseDuration);

        SceneManager.LoadScene(gameSceneName);
    }

    // Una sola corrutina de fade
    IEnumerator Fade(float desde, float hasta, float duracion)
    {
        float timer = 0f;
        while (timer < duracion)
        {
            timer += Time.unscaledDeltaTime;
            SetFadeAlpha(Mathf.Lerp(desde, hasta, timer / duracion));
            yield return null;
        }
        SetFadeAlpha(hasta);
    }

    void SetFadeAlpha(float alpha)
    {
        if (fadeImage == null) return;

        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}