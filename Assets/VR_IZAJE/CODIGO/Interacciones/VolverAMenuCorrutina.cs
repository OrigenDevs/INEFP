using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverAMenuCorrutina : MonoBehaviour
{
    [SerializeField] private string nombreEscena = "HOME";
    [SerializeField] private float segundosEspera = 5f;

    private bool corrutinaActiva = false;

    public void Iniciar()
    {
        if (!corrutinaActiva)
            StartCoroutine(CuentaAtras());
    }

    private System.Collections.IEnumerator CuentaAtras()
    {
        corrutinaActiva = true;
        yield return new WaitForSeconds(segundosEspera);
        SceneManager.LoadScene(nombreEscena);
    }
}
