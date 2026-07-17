using UnityEngine;
using System.Collections;

public class TransicionVr : MonoBehaviour
{
    [SerializeField] private float duracionDefecto = 0.5f;
    public Renderer esferaFade;
    public GameObject objetoATransportar;
    public Transform destino;
    public AudioSource audioTransicion;
    public bool autoIniciarAlActivar;

    private static TransicionVr instancia;
    private Material materialInstancia;
    private string propiedadColor = "_Color";
    private Coroutine rutinaActual;

    void Awake()
    {
        instancia = this;
        if (esferaFade == null)
        {
            Debug.LogError("TransicionVr: esferaFade no asignado en " + name);
            return;
        }
        materialInstancia = new Material(esferaFade.sharedMaterial);
        esferaFade.material = materialInstancia;
        if (materialInstancia.HasProperty("_BaseColor"))
            propiedadColor = "_BaseColor";
        Color c = materialInstancia.GetColor(propiedadColor);
        Debug.Log($"TransicionVr: Iniciada, material={esferaFade.sharedMaterial.name}, propiedad={propiedadColor}, alpha={c.a}");
        c.a = 0f;
        materialInstancia.SetColor(propiedadColor, c);
    }

    void OnEnable()
    {
        if (autoIniciarAlActivar)
            ParpadearConTransporte(duracionDefecto, duracionDefecto);
    }

    void OnDisable()
    {
        if (rutinaActual != null)
            StopCoroutine(rutinaActual);
    }

    private float ObtenerAlpha()
    {
        return materialInstancia.GetColor(propiedadColor).a;
    }

    private void FijarAlpha(float a)
    {
        Color c = materialInstancia.GetColor(propiedadColor);
        c.a = Mathf.Clamp01(a);
        materialInstancia.SetColor(propiedadColor, c);
    }

    private IEnumerator RutinaFundido(float desde, float hasta, float duracion)
    {
        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            FijarAlpha(Mathf.Lerp(desde, hasta, t / duracion));
            yield return null;
        }
        FijarAlpha(hasta);
    }

    public static void FundidoANegro(float duracion)
    {
        if (instancia?.materialInstancia == null) { Debug.LogWarning("TransicionVr.FundidoANegro: nulo"); return; }
        instancia.Detener();
        float desde = instancia.ObtenerAlpha();
        instancia.rutinaActual = instancia.StartCoroutine(instancia.RutinaFundido(desde, 1f, duracion));
    }

    public static void AparecerDesdeNegro(float duracion)
    {
        if (instancia?.materialInstancia == null) { Debug.LogWarning("TransicionVr.AparecerDesdeNegro: nulo"); return; }
        instancia.Detener();
        float desde = instancia.ObtenerAlpha();
        instancia.rutinaActual = instancia.StartCoroutine(instancia.RutinaFundido(desde, 0f, duracion));
    }

    public static void Parpadear(float duracionFundido, float espera)
    {
        if (instancia?.materialInstancia == null) { Debug.LogWarning("TransicionVr.Parpadear: nulo"); return; }
        instancia.Detener();
        instancia.rutinaActual = instancia.StartCoroutine(instancia.RutinaParpadear(duracionFundido, espera));
    }

    private IEnumerator RutinaParpadear(float duracionFundido, float espera)
    {
        yield return RutinaFundido(ObtenerAlpha(), 1f, duracionFundido);
        yield return new WaitForSeconds(espera);
        yield return RutinaFundido(ObtenerAlpha(), 0f, duracionFundido);
    }

    public static void ParpadearConTransporte()
    {
        if (instancia == null) return;
        ParpadearConTransporte(instancia.duracionDefecto, instancia.duracionDefecto);
    }

    public static void ParpadearConTransporte(float duracionFundido, float espera)
    {
        if (instancia?.materialInstancia == null) { Debug.LogWarning("TransicionVr.ParpadearConTransporte: nulo"); return; }
        instancia.Detener();
        instancia.rutinaActual = instancia.StartCoroutine(instancia.RutinaParpadearConTransporte(duracionFundido, espera));
    }

    private IEnumerator RutinaParpadearConTransporte(float duracionFundido, float espera)
    {
        yield return RutinaFundido(ObtenerAlpha(), 1f, duracionFundido);
        Debug.Log("TransicionVr: Transportando objeto");
        if (objetoATransportar != null && destino != null)
        {
            objetoATransportar.transform.SetPositionAndRotation(destino.position, destino.rotation);
            Debug.Log($"TransicionVr: Objeto movido a {destino.position}");
        }
        if (audioTransicion != null)
            audioTransicion.Play();
        yield return new WaitForSeconds(espera);
        yield return RutinaFundido(ObtenerAlpha(), 0f, duracionFundido);
        Debug.Log("TransicionVr: Completado, desactivando");
        gameObject.SetActive(false);
    }

    public static void ApagarAhora()
    {
        if (instancia?.materialInstancia == null) { Debug.LogWarning("TransicionVr.ApagarAhora: nulo"); return; }
        instancia.Detener();
        instancia.FijarAlpha(0f);
    }

    private void Detener()
    {
        if (rutinaActual != null)
        {
            StopCoroutine(rutinaActual);
            rutinaActual = null;
        }
    }
}
