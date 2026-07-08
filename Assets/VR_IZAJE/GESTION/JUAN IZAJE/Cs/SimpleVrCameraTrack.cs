using UnityEngine;
using UnityEngine.InputSystem.XR; // Necesario para acceder al Tracked Pose Driver

public class ConfigureCameraForCoaster : MonoBehaviour
{
    void Awake()
    {
        // Buscamos el componente Tracked Pose Driver en la cámara
        TrackedPoseDriver trackedPoseDriver = GetComponent<TrackedPoseDriver>();

        if (trackedPoseDriver != null)
        {
            // Forzamos a que en esta escena SOLO use la rotación (ignora el WASD de posición del simulador y el movimiento físico)
            trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
            Debug.Log("[Montaña Rusa] Cámara configurada: SOLO ROTACIÓN activa.");
        }
        else
        {
            Debug.LogWarning("No se encontró el componente TrackedPoseDriver en este GameObject.");
        }
    }
}