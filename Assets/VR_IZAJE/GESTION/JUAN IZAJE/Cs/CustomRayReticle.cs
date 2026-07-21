using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRRayInteractor))]
public class CustomRayReticle : MonoBehaviour
{
    [Header("Configuración de Retícula")]
    [Tooltip("El objeto o prefab del Quad que se usará como retícula.")]
    public Transform reticleObject;

    [Tooltip("Distancia que se separa la retícula hacia atrás para evitar que se atraviese en la UI o paredes.")]
    public float surfaceOffset = 0.01f;

    [Tooltip("Color cuando el rayo apunta a un objeto válido interactuable o UI.")]
    public Color validColor = Color.green;

    [Tooltip("Color cuando el rayo apunta al aire o a un objeto no válido.")]
    public Color invalidColor = Color.white;

    private XRRayInteractor rayInteractor;
    private Renderer reticleRenderer;

    void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        if (reticleObject != null)
        {
            reticleRenderer = reticleObject.GetComponent<Renderer>();
        }
    }

    void Update()
    {
        if (reticleObject == null) return;

        // Si ya tenemos un objeto agarrado, ocultamos la retícula
        if (rayInteractor.hasSelection)
        {
            reticleObject.gameObject.SetActive(false);
            return;
        }

        RaycastHit hit3D;
        bool hasHit3D = rayInteractor.TryGetCurrent3DRaycastHit(out hit3D);

        if (hasHit3D)
        {
            // Caso 1: Choca con un objeto físico 3D (aplicamos offset a lo largo de la normal)
            reticleObject.gameObject.SetActive(true);
            reticleObject.position = hit3D.point + (hit3D.normal * surfaceOffset);
            reticleObject.rotation = Quaternion.LookRotation(hit3D.normal);

            bool isValidTarget = rayInteractor.hasHover;
            if (reticleRenderer != null)
            {
                reticleRenderer.material.color = isValidTarget ? validColor : invalidColor;
            }
        }
        else
        {
            // Caso 2: No chocó en 3D, probamos si está chocando con la UI
            UnityEngine.EventSystems.RaycastResult uiHit;
            bool hasHitUI = rayInteractor.TryGetCurrentUIRaycastResult(out uiHit);

            if (hasHitUI)
            {
                reticleObject.gameObject.SetActive(true);

                Vector3 normal = uiHit.worldNormal != Vector3.zero ? uiHit.worldNormal : -transform.forward;
                // Aplicamos el offset en la UI también para que el Quad flote frente al Canvas
                reticleObject.position = uiHit.worldPosition + (normal * surfaceOffset);
                reticleObject.rotation = Quaternion.LookRotation(normal);

                // La UI interactiva con la que choca se considera un blanco válido
                if (reticleRenderer != null)
                {
                    reticleRenderer.material.color = validColor;
                }
            }
            else
            {
                // Si no choca con nada (ni 3D ni UI), la ocultamos
                reticleObject.gameObject.SetActive(false);
            }
        }
    }
}