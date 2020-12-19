using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAttach : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        this.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
