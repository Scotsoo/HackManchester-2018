using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackScript : MonoBehaviour
{

    public Material noLook;

    public Material eyeSpy;
	// Use this for initialization
	void Start () {
		   
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NoLook()
    {
        GetComponent<Renderer>().material = noLook;
    }

    public void Look()
    {
        GetComponent<Renderer>().material = eyeSpy;
    }
}
