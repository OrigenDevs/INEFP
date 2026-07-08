using UnityEngine;

public class SteeringWheelLimiter : MonoBehaviour
{
    public enum SteeringAxis { X, Y, Z }

    [Header("References")]
    [Tooltip("El objeto padre que se mueve con la Spline.")]
    [SerializeField] private Transform truckTransform;

    [Header("Steering Settings")]
    [Tooltip("¿Qué eje local debe girar para simular la dirección del volante?")]
    [SerializeField] private SteeringAxis steeringAxis = SteeringAxis.Z;

    [Tooltip("Factor de división. Más alto = el volante gira menos en las curvas.")]
    [SerializeField] private float rotationDivider = 4f;

    [Tooltip("Ángulo máximo que puede girar el volante a cada lado (grados).")]
    [SerializeField] private float maxSteeringAngle = 60f;

    [Tooltip("Velocidad a la que el volante sigue al target de giro.")]
    [SerializeField] private float turnSpeed = 8f;

    [Tooltip("Velocidad a la que el volante vuelve al centro.")]
    [SerializeField] private float returnSpeed = 30f;

    [Tooltip("Ángulo mínimo de giro para considerar que el camión está en curva.")]
    [SerializeField] private float deadZone = 0.2f;

    private float currentWheelRotation = 0f;
    private float targetWheelRotation = 0f;
    private Vector3 lastTruckDir;
    private Quaternion initialLocalRotation;

    private void Start()
    {
        if (truckTransform != null)
            lastTruckDir = GetTruckForward();

        initialLocalRotation = transform.localRotation;
    }

    private Vector3 GetTruckForward()
    {
        return truckTransform.right;
    }

    private void LateUpdate()
    {
        if (truckTransform == null) return;

        Vector3 currentDir = GetTruckForward();
        float deltaRotation = Vector3.SignedAngle(lastTruckDir, currentDir, truckTransform.up);
        lastTruckDir = currentDir;

        float absDelta = Mathf.Abs(deltaRotation);

        targetWheelRotation = Mathf.MoveTowards(targetWheelRotation, 0f, Time.deltaTime * returnSpeed * 0.5f);

        if (absDelta > deadZone)
            targetWheelRotation += (deltaRotation / rotationDivider);

        targetWheelRotation = Mathf.Clamp(targetWheelRotation, -maxSteeringAngle, maxSteeringAngle);

        float speed = absDelta > deadZone ? turnSpeed : returnSpeed;
        currentWheelRotation = Mathf.MoveTowards(currentWheelRotation, targetWheelRotation, Time.deltaTime * speed);

        Vector3 steeringVector = Vector3.zero;
        switch (steeringAxis)
        {
            case SteeringAxis.X: steeringVector.x = currentWheelRotation; break;
            case SteeringAxis.Y: steeringVector.y = currentWheelRotation; break;
            case SteeringAxis.Z: steeringVector.z = currentWheelRotation; break;
        }

        transform.localRotation = initialLocalRotation * Quaternion.Euler(steeringVector);
    }
}
