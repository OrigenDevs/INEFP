using UnityEngine;

public class PreviewGizmo : MonoBehaviour
{
    public Transform objetoAPrevisualizar;

    void OnDrawGizmos()
    {
        if (objetoAPrevisualizar == null) return;

        Vector3 delta = transform.position - objetoAPrevisualizar.position;
        Quaternion rotDelta = transform.rotation * Quaternion.Inverse(objetoAPrevisualizar.rotation);

        DrawChildren(objetoAPrevisualizar, delta, rotDelta);
    }

    void DrawChildren(Transform t, Vector3 delta, Quaternion rotDelta)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);

            var mf = child.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
            {
                Vector3 nuevaPos = rotDelta * (child.position - objetoAPrevisualizar.position) + transform.position;
                Quaternion nuevaRot = rotDelta * child.rotation;

                var mr = child.GetComponent<MeshRenderer>();
                Gizmos.color = mr != null && mr.sharedMaterial != null ? new Color(mr.sharedMaterial.color.r, mr.sharedMaterial.color.g, mr.sharedMaterial.color.b, 0.4f) : new Color(0, 1, 1, 0.4f);

                Gizmos.DrawWireMesh(mf.sharedMesh, nuevaPos, nuevaRot, child.lossyScale);
            }

            DrawChildren(child, delta, rotDelta);
        }
    }
}
