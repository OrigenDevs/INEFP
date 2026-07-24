using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SequenceGlowControllerVR : MonoBehaviour
{
    [Header("Material base del brillo")]
    [SerializeField] private Material baseMaterial;

    [Header("Imágenes en orden (0 al 6)")]
    [SerializeField] private Image[] targetImages;

    [Header("Configuración de Animación")]
    [SerializeField] private float scaleSpeed = 12f; // Velocidad de transición

    private static readonly int CycleTimeProp = Shader.PropertyToID("_CycleTime");
    private static readonly int StartTimeProp = Shader.PropertyToID("_StartTime");

    // Guardamos las referencias a las corrutinas activas para evitar conflictos
    private Coroutine[] scaleCoroutines;

    private void Awake()
    {
        if (baseMaterial == null)
        {
            Debug.LogError("¡Falta asignar el Material Base en el Inspector!", this);
            return;
        }

        scaleCoroutines = new Coroutine[targetImages.Length];

        for (int i = 0; i < targetImages.Length; i++)
        {
            if (targetImages[i] != null)
            {
                // Instancia única por imagen
                targetImages[i].material = new Material(baseMaterial);
                targetImages[i].material.SetFloat(CycleTimeProp, 0f);
                targetImages[i].transform.localScale = Vector3.one;
            }
        }
    }

    public void ActivarBrillo(int index)
    {
        ApagarTodas();

        if (index >= 0 && index < targetImages.Length && targetImages[index] != null)
        {
            Material mat = targetImages[index].material;

            // Reiniciamos shader y activamos brillo
            mat.SetFloat(StartTimeProp, Time.time);
            mat.SetFloat(CycleTimeProp, 2f);

            // Animamos la escala de forma eficiente con corrutina
            if (scaleCoroutines[index] != null) StopCoroutine(scaleCoroutines[index]);
            scaleCoroutines[index] = StartCoroutine(AnimateScale(targetImages[index].transform, Vector3.one * 1.2f));
        }
    }

    public void ApagarTodas()
    {
        for (int i = 0; i < targetImages.Length; i++)
        {
            if (targetImages[i] != null && targetImages[i].material != null)
            {
                targetImages[i].material.SetFloat(CycleTimeProp, 0f);

                // Devolvemos la escala a la normalidad de forma suave
                if (scaleCoroutines[i] != null) StopCoroutine(scaleCoroutines[i]);
                scaleCoroutines[i] = StartCoroutine(AnimateScale(targetImages[i].transform, Vector3.one));
            }
        }
    }

    private IEnumerator AnimateScale(Transform targetTransform, Vector3 targetScale)
    {
        while (Vector3.Distance(targetTransform.localScale, targetScale) > 0.001f)
        {
            targetTransform.localScale = Vector3.Lerp(targetTransform.localScale, targetScale, Time.deltaTime * scaleSpeed);
            yield return null; // Espera al siguiente frame solo mientras se anima
        }
        targetTransform.localScale = targetScale; // Asegura el valor exacto al terminar
    }
}