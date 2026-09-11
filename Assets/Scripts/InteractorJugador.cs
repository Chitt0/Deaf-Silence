using UnityEngine;

public class InteractorJugador : MonoBehaviour
{
    [SerializeField] private Transform puntoMano;

    private Puerta puertaCercana;
    private Linterna linternaCercana;
    private Linterna linternaEnMano;
    private InputSystem_Actions controles;

    void Start()
    {
        controles = new InputSystem_Actions();
        controles.Player.Enable();

        controles.Player.Interact.performed += ctx => Interactuar();
        controles.Player.Attack.performed += ctx => AlternarLinterna();
    }

    public void RegistrarPuertaCercana(Puerta puerta)
    {
        puertaCercana = puerta;
    }

    public void QuitarPuertaCercana(Puerta puerta)
    {
        if (puertaCercana == puerta)
        {
            puertaCercana = null;
        }
    }

    public void RegistrarLinternaCercana(Linterna linterna)
    {
        linternaCercana = linterna;
    }

    public void QuitarLinternaCercana(Linterna linterna)
    {
        if (linternaCercana == linterna)
        {
            linternaCercana = null;
        }
    }

    private void Interactuar()
    {
        if (linternaCercana != null)
        {
            linternaEnMano = linternaCercana;
            linternaCercana.Recoger(puntoMano);
            linternaCercana = null;
            return;
        }

        if (puertaCercana != null)
        {
            StartCoroutine(CruzarPuerta(puertaCercana));
        }
    }

    private void AlternarLinterna()
    {
        if (linternaEnMano != null)
        {
            linternaEnMano.AlternarEncendido();
        }
    }

    private System.Collections.IEnumerator CruzarPuerta(Puerta puerta)
    {
        puerta.ReproducirSonido();

        yield return ControladorFade.Instancia.FundirANegro();

        transform.position = puerta.ObtenerPosicionDestino();
        transform.rotation = puerta.ObtenerRotacionDestino();

        if (puerta.CuentaComoVuelta())
        {
            ContadorVueltas.Instancia.SumarVuelta();
        }

        yield return ControladorFade.Instancia.FundirDesdeNegro();
    }
}