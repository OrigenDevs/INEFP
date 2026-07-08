using UnityEngine;

public class SoloCamara : MonoBehaviour
{
    private Vector3 posLocalInicial;
    private Quaternion rotLocalInicial;
    private CharacterController cc;
    private Rigidbody rb;

    void Start()
    {
        posLocalInicial = transform.localPosition;
        rotLocalInicial = transform.localRotation;

        cc = GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }

    void LateUpdate()
    {
        transform.localPosition = posLocalInicial;
        transform.localRotation = rotLocalInicial;
    }

    void FixedUpdate()
    {
        transform.localPosition = posLocalInicial;
        transform.localRotation = rotLocalInicial;
    }
}
