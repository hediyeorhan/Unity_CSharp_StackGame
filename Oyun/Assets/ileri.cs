using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ileri : MonoBehaviour
{
    
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 position = this.transform.position;
       position.z += 0.1f;
        this.transform.position = position;
        
    }
}
