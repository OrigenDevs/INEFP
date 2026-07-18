using UnityEngine;

public class OverrideRotacionFlecha : MonoBehaviour
{
    private Quaternion rotacionInicial;

    void Start()
    {
        rotacionInicial = transform.localRotation;
    }

    void LateUpdate()
    {
        transform.localRotation = rotacionInicial;
    }
}
