using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class FlechaSeguimientoCollider : MonoBehaviour
{
    [System.Serializable]
    public class ParObjetos
    {
        public GameObject objetoASeguir;
        public UnityEvent onObjetoSalio;
    }

    public Collider colliderMaestro;
    public List<ParObjetos> pares = new List<ParObjetos>();

    public UnityEvent onColliderExit;

    void Start()
    {
        if (colliderMaestro == null)
            colliderMaestro = GetComponent<Collider>();
    }

    void OnTriggerExit(Collider other)
    {
        foreach (var par in pares)
        {
            if (par.objetoASeguir != null && par.objetoASeguir == other.gameObject)
            {
                par.onObjetoSalio?.Invoke();
                break;
            }
        }

        onColliderExit?.Invoke();
    }
}
