using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private Transform destino;
    [SerializeField] private AudioSource sonido;

    private void OnTriggerEnter(Collider other)
    {
        GameObject root = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;
        Transform player = root.transform;

        while (player != null && player.GetComponent<CharacterController>() == null)
            player = player.parent;

        if (player == null)
            player = root.transform.root;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.SetPositionAndRotation(destino.position, destino.rotation);
        Physics.SyncTransforms();

        if (cc != null) cc.enabled = true;

        if (sonido != null)
            sonido.Play();
    }
}
