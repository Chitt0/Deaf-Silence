using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class SubtitleLine
{
    public string speaker;           // Opcional: "Madre", "Voz", etc.
    [TextArea(2, 4)]
    public string text;
    public float duration = 3f;
}

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI subtitleText;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        panel.SetActive(false);
    }

    // Muestra una sola línea
    public void ShowLine(SubtitleLine line)
    {
        ShowSequence(new SubtitleLine[] { line });
    }

    // Muestra varias líneas una tras otra
    public void ShowSequence(SubtitleLine[] lines)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PlaySequence(lines));
    }

    public void Hide()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        panel.SetActive(false);
    }

    private IEnumerator PlaySequence(SubtitleLine[] lines)
    {
        panel.SetActive(true);

        foreach (SubtitleLine line in lines)
        {
            if (string.IsNullOrEmpty(line.speaker))
                subtitleText.text = line.text;
            else
                subtitleText.text = $"<b>{line.speaker}:</b> {line.text}";

            yield return new WaitForSeconds(line.duration);
        }

        panel.SetActive(false);
        currentRoutine = null;
    }
}