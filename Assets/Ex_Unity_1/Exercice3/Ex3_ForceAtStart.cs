using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex3_ForceAtStart : MonoBehaviour
{
    public float MinInitialForce = 20f;
    public float MaxInitialForce = 90f;

    private void Start()
    {
        Invoke("Launch", 1f);
    }

    private void Launch()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(MinInitialForce, MaxInitialForce), ForceMode.Impulse);
    }
}
