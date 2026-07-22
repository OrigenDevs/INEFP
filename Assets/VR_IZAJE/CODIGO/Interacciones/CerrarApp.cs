using UnityEngine;

public class CerrarApp : MonoBehaviour
{
    public void Cerrar()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
