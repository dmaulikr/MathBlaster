using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroy : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (gameObject.GetComponent<ParticleSystem>())
        {
            GameObject.Destroy(gameObject, gameObject.GetComponent<ParticleSystem>().duration + gameObject.GetComponent<ParticleSystem>().startLifetime);
        }
    }
}
