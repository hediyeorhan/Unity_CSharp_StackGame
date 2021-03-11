using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    public float hiz;
    // Start is called before the first frame update
    void Start()
    {

        hiz = 5f;
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(hiz*Input.GetAxis("Horizontal")* Time.deltaTime, 0f, 0f);

    }
}
