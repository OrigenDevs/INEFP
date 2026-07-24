using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequenceDelayActivator : MonoBehaviour
{
    [System.Serializable]
    public struct DelayedObject
    {
        [Tooltip("El objeto que se va a activar")]
        public GameObject targetObject;

        [Tooltip("Segundos exactos desde que inicia la secuencia hasta que se activa este objeto")]
        public float delayTime;
    }

    [Header("Secuencia de Activación")]
    [SerializeField] private List<DelayedObject> activationSequence = new List<DelayedObject>();

    // Esta función la llamas una sola vez desde tu evento principal
    public void IniciarActivacion()
    {
        StopAllCoroutines();

        // Lanzamos una corrutina independiente para cada objeto de la lista
        foreach (var item in activationSequence)
        {
            if (item.targetObject != null)
            {
                StartCoroutine(ActivarObjetoIndividual(item.targetObject, item.delayTime));
            }
        }
    }

    private IEnumerator ActivarObjetoIndividual(GameObject objetoAActivar, float tiempoEspera)
    {
        // Cada objeto espera su propio tiempo de forma independiente desde el inicio
        if (tiempoEspera > 0f)
        {
            yield return new WaitForSeconds(tiempoEspera);
        }

        objetoAActivar.SetActive(true);
    }
}