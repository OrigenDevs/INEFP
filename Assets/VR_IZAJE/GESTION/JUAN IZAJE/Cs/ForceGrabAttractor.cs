using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // Necesario para XRGrabInteractable en versiones nuevas
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class ForceGrabAttractor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float attractionSpeed = 10f;

    private XRGrabInteractable grabInteractable;
    private IXRInteractor currentInteractor;
    private bool isAttracting = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Suscribirse a los eventos del interactuable
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Solo atraemos si el interactor es de tipo Ray o Near-Far
        if (args.interactorObject is XRRayInteractor || args.interactorObject is NearFarInteractor)
        {
            currentInteractor = args.interactorObject;
            isAttracting = true;
        }
    }

    void Update()
    {
        if (isAttracting && currentInteractor != null)
        {
            // Mover el objeto hacia el transform de agarre del interactor
            Transform target = currentInteractor.GetAttachTransform(grabInteractable);
            transform.position = Vector3.Lerp(transform.position, target.position, attractionSpeed * Time.deltaTime);

            // Si estamos lo suficientemente cerca, detenemos la atracción
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                isAttracting = false;
            }
        }
    }
}