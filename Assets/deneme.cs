using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Deneme : MonoBehaviour
{
    public Rigidbody rb2;
    
    public Rigidbody targetRb;
    

    // Start is called before the first frame update


    private void Awake()
    {
        //rb2 = GetComponent<Rigidbody>();
        //GameObject targetObject = GameObject.Find("aman");
        //targetRb = targetObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      
        rb2.velocity = Vector3.up;
        targetRb.velocity = Vector3.up;

    }
}
