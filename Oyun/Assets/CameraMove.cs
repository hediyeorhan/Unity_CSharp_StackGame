using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    GameObject playerObj;
    Vector3 cameraOffSet;

    // Use this for initialization
    void Start ()
    {
        playerObj = GameObject.Find("Player");
        cameraOffSet = new Vector3(0, 2, -6);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = playerObj.transform.position + cameraOffSet;
	}
}
