using UnityEngine;

public class DesactivarFog : MonoBehaviour
{
    public void Desactivar()
    {
        RenderSettings.fog = false;
    }

    public void Activar()
    {
        RenderSettings.fog = true;
    }
}
