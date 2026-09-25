using UnityEngine;

public class ControladorReticula : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private RectTransform reticulaRect;
    [SerializeField] private Transform camara;

    [Header("Tamaños")]
    [SerializeField] private Vector2 tamanoPunto = new Vector2(8f, 8f);
    [SerializeField] private Vector2 tamanoCirculo = new Vector2(20f, 20f);

    [Header("Detección")]
    [SerializeField] private float distanciaInteraccion = 3f;

    void Update()
    {
        bool apuntandoAInteractuable = DetectarInteractuable();

        reticulaRect.sizeDelta = apuntandoAInteractuable ? tamanoCirculo : tamanoPunto;
    }

    private bool DetectarInteractuable()
    {
        RaycastHit hit;
        if (Physics.Raycast(camara.position, camara.forward, out hit, distanciaInteraccion))
        {
            if (hit.collider.GetComponent<Puerta>() != null) return true;
            if (hit.collider.GetComponent<Linterna>() != null) return true;
        }
        return false;
    }
}