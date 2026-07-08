using UnityEngine;

public class SeguirVehiculo : MonoBehaviour
{
    public Transform vehiculo;
    public Vector3 offset = Vector3.zero;
    public bool seguirRotacion = false;

    void LateUpdate()
    {
        if (vehiculo == null) return;
        transform.position = vehiculo.position + offset;
        if (seguirRotacion)
            transform.rotation = vehiculo.rotation;
    }
}
