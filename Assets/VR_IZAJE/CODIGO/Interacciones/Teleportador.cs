using UnityEngine;

public class Teleportador : MonoBehaviour
{
    [SerializeField] private Transform destino;

    public void Teleportar()
    {
        CharacterController cc = FindFirstObjectByType<CharacterController>();
        if (cc == null) return;

        cc.enabled = false;
        cc.transform.SetParent(null);
        cc.transform.SetPositionAndRotation(destino.position, destino.rotation);
        Physics.SyncTransforms();
        cc.enabled = true;
    }
}
