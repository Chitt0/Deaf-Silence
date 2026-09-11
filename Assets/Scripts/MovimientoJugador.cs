using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Referencias")]
    private Rigidbody rb;
    private InputSystem_Actions controles;

    [Header("Movimiento")]
    [SerializeField] private float velocidadCaminar = 3f;
    [SerializeField] private float velocidadCorrer = 5f;
    [SerializeField] private float aceleracion = 8f;
    [SerializeField] private float desaceleracion = 10f;

    [Header("Cámara")]
    [SerializeField] private Transform camara;
    [SerializeField] private float sensibilidad = 0.1f;
    [SerializeField] private float limiteVerticalMin = -80f; // Abajo / Mirar hacia arriba
    [SerializeField] private float limiteVerticalMax = 80f;  // Arriba / Mirar hacia abajo
    [SerializeField] private float pesoCamara = 0.08f;

    [Header("Sacudida al caminar (Head Bob)")]
    [SerializeField] private float bobFrecuenciaCaminar = 8f;
    [SerializeField] private float bobFrecuenciaCorrer = 14f;
    [SerializeField] private float bobAmplitudVerticalCaminar = 0.05f;
    [SerializeField] private float bobAmplitudVerticalCorrer = 0.09f;
    [SerializeField] private float bobAmplitudHorizontalCaminar = 0.03f;
    [SerializeField] private float bobAmplitudHorizontalCorrer = 0.06f;
    [SerializeField] private float bobInclinacionCaminar = 1.5f;
    [SerializeField] private float bobInclinacionCorrer = 2f;
    [SerializeField] private float suavizadoBob = 10f;

    private Vector2 inputMovimiento;
    private Vector2 inputMirar;
    private Vector2 inputMirarActual;
    private Vector2 velocidadMirarActual;
    private bool corriendo;
    private Vector3 velocidadActual;
    private float rotacionVertical;

    private Vector3 posicionCamaraOriginal;
    private float bobTimer;
    private float inclinacionBobActual;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controles = new InputSystem_Actions();

        controles.Player.Enable();

        controles.Player.Move.performed += ctx => inputMovimiento = ctx.ReadValue<Vector2>();
        controles.Player.Move.canceled += ctx => inputMovimiento = Vector2.zero;

        controles.Player.Sprint.performed += ctx => corriendo = true;
        controles.Player.Sprint.canceled += ctx => corriendo = false;

        controles.Player.Look.performed += ctx => inputMirar = ctx.ReadValue<Vector2>();
        controles.Player.Look.canceled += ctx => inputMirar = Vector2.zero;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (camara != null)
        {
            posicionCamaraOriginal = camara.localPosition;
            
            // Sincronizar la rotación vertical inicial con la transformación del editor
            float anguloX = camara.localEulerAngles.x;
            if (anguloX > 180f) anguloX -= 360f;
            rotacionVertical = anguloX;
        }
    }

    void Update()
{
    // --- 1. Control y verificación del bloqueo de cursor ---
    // Si haces clic en la ventana de Game, asegura el bloqueo
    if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Limpiar inmediatamente cualquier delta del mouse acumulado antes del clic
        inputMirar = Vector2.zero;
        inputMirarActual = Vector2.zero;
        velocidadMirarActual = Vector2.zero;
    }

    // Si el cursor no está bloqueado (ej. interactuando con la interfaz o antes del primer clic),
    // no procesamos la rotación de la cámara.
    if (Cursor.lockState == CursorLockMode.Locked)
    {
        // --- Giro de cámara ---
        inputMirarActual = Vector2.SmoothDamp(inputMirarActual, inputMirar, ref velocidadMirarActual, pesoCamara);

        float rotacionHorizontal = inputMirarActual.x * sensibilidad;
        transform.Rotate(Vector3.up * rotacionHorizontal);

        rotacionVertical -= inputMirarActual.y * sensibilidad;
        rotacionVertical = Mathf.Clamp(rotacionVertical, limiteVerticalMin, limiteVerticalMax);
    }
    else
    {
        // Si el cursor se libera (por ejemplo al presionar ESC), reseteamos los buffers
        inputMirarActual = Vector2.zero;
        velocidadMirarActual = Vector2.zero;
    }

    // --- 2. Head bob y aplicación de rotaciones/posiciones ---
    bool moviendose = inputMovimiento.magnitude > 0.1f;
    Vector3 offsetBob = Vector3.zero;
    float inclinacionObjetivo = 0f;

    if (moviendose)
    {
        float frecuencia = corriendo ? bobFrecuenciaCorrer : bobFrecuenciaCaminar;
        float amplitudVertical = corriendo ? bobAmplitudVerticalCorrer : bobAmplitudVerticalCaminar;
        float amplitudHorizontal = corriendo ? bobAmplitudHorizontalCorrer : bobAmplitudHorizontalCaminar;
        float inclinacionMax = corriendo ? bobInclinacionCorrer : bobInclinacionCaminar;

        bobTimer += Time.deltaTime * frecuencia;

        float bobVertical = Mathf.Sin(bobTimer * 2f) * amplitudVertical;
        float bobHorizontal = Mathf.Cos(bobTimer) * amplitudHorizontal;

        offsetBob = new Vector3(bobHorizontal, bobVertical, 0f);
        inclinacionObjetivo = Mathf.Cos(bobTimer) * inclinacionMax;
    }
    else
    {
        bobTimer = 0f;
    }

    inclinacionBobActual = Mathf.Lerp(inclinacionBobActual, inclinacionObjetivo, Time.deltaTime * suavizadoBob);

    camara.localPosition = Vector3.Lerp(camara.localPosition, posicionCamaraOriginal + offsetBob, Time.deltaTime * suavizadoBob);
    camara.localRotation = Quaternion.Euler(rotacionVertical, 0f, inclinacionBobActual);

    if (TryGetComponent<SonidoPasos>(out var pasos))
    {
        pasos.ActualizarEstado(inputMovimiento, corriendo);
    }
}

    void FixedUpdate()
    {
        Vector3 direccionDeseada = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;
        direccionDeseada.Normalize();

        float velocidadObjetivo = corriendo ? velocidadCorrer : velocidadCaminar;
        Vector3 velocidadDeseada = direccionDeseada * velocidadObjetivo;

        float tasaDeCambio = (inputMovimiento.magnitude > 0.1f) ? aceleracion : desaceleracion;
        velocidadActual = Vector3.MoveTowards(velocidadActual, velocidadDeseada, tasaDeCambio * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector3(velocidadActual.x, rb.linearVelocity.y, velocidadActual.z);

        if (TryGetComponent<RespiracionJugador>(out var respiracion))
        {
            respiracion.ActualizarEstado(corriendo);
        }
    }

    private void OnEnable()
    {
        controles?.Player.Enable();
    }

    private void OnDisable()
    {
        controles?.Player.Disable();
    }
}