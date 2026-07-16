using UnityEngine;
using DG.Tweening;

public class TransicionVr : MonoBehaviour
{
    [SerializeField] private float duracionDefecto = 0.5f;
    public Renderer esferaFade;
    public GameObject objetoATransportar;
    public Transform destino;
    public AudioSource audioTransicion;
    public bool autoIniciarAlActivar;

    private static TransicionVr instancia;
    private Material materialFade;
    private string propiedadColor = "_Color";
    private Sequence secuenciaActual;
    private static bool dotweenInicializado;

    void Awake()
    {
        if (!dotweenInicializado)
        {
            DOTween.Init();
            dotweenInicializado = true;
        }
        instancia = this;
        if (esferaFade == null)
        {
            Debug.LogError("TransicionVr: esferaFade no asignada en " + name);
            return;
        }
        materialFade = esferaFade.material;
        if (materialFade.HasProperty("_BaseColor"))
            propiedadColor = "_BaseColor";
        Debug.Log($"TransicionVr: Iniciada, esfera={esferaFade.name}, propiedadColor={propiedadColor}, alpha inicial={materialFade.GetColor(propiedadColor).a}");
        Color c = materialFade.GetColor(propiedadColor);
        c.a = 0f;
        materialFade.SetColor(propiedadColor, c);
    }

    void OnEnable()
    {
        if (autoIniciarAlActivar)
            ParpadearConTransporte(duracionDefecto, duracionDefecto);
    }

    void OnDisable()
    {
        secuenciaActual?.Kill();
        if (materialFade != null)
            DOTween.Kill(materialFade);
    }

    public static void FundidoANegro(float duracion)
    {
        if (instancia?.materialFade == null) return;
        DOTween.Kill(instancia.materialFade);
        instancia.materialFade.DOFade(1f, instancia.propiedadColor, duracion);
    }

    public static void AparecerDesdeNegro(float duracion)
    {
        if (instancia?.materialFade == null) return;
        DOTween.Kill(instancia.materialFade);
        instancia.materialFade.DOFade(0f, instancia.propiedadColor, duracion);
    }

    public static void Parpadear(float duracionFundido, float espera)
    {
        if (instancia?.materialFade == null) return;
        Sequence seq = DOTween.Sequence();
        seq.Append(instancia.materialFade.DOFade(1f, instancia.propiedadColor, duracionFundido));
        seq.AppendInterval(espera);
        seq.Append(instancia.materialFade.DOFade(0f, instancia.propiedadColor, duracionFundido));
    }

    public static void ParpadearConTransporte()
    {
        ParpadearConTransporte(instancia.duracionDefecto, instancia.duracionDefecto);
    }

    public static void ParpadearConTransporte(float duracionFundido, float espera)
    {
        if (instancia?.materialFade == null) return;

        instancia.secuenciaActual?.Kill();
        DOTween.Kill(instancia.materialFade);

        Sequence seq = DOTween.Sequence();
        instancia.secuenciaActual = seq;
        seq.Append(instancia.materialFade.DOFade(1f, instancia.propiedadColor, duracionFundido));
        seq.AppendCallback(() =>
        {
            if (instancia.objetoATransportar != null && instancia.destino != null)
                instancia.objetoATransportar.transform.SetPositionAndRotation(instancia.destino.position, instancia.destino.rotation);
            if (instancia.audioTransicion != null)
                instancia.audioTransicion.Play();
        });
        seq.AppendInterval(espera);
        seq.Append(instancia.materialFade.DOFade(0f, instancia.propiedadColor, duracionFundido));
        seq.OnComplete(() =>
        {
            if (instancia != null)
                instancia.gameObject.SetActive(false);
        });
    }

    public static void ApagarAhora()
    {
        if (instancia?.materialFade == null) return;
        instancia.secuenciaActual?.Kill();
        DOTween.Kill(instancia.materialFade);
        Color c = instancia.materialFade.GetColor(instancia.propiedadColor);
        c.a = 0f;
        instancia.materialFade.SetColor(instancia.propiedadColor, c);
    }
}
