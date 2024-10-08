using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneme : MonoBehaviour
{

    public Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
       // body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = Vector3.up;
    }
}
