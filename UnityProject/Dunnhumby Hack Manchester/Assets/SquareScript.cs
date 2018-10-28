using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnLook ()
    {
        Debug.Log("LOOKING!!");
    }

    public void OnClick()
    {
        var movieId = gameObject.GetComponent<TapeMetadata>().MovieMetadata.Id;
        Debug.Log("CLICKING!!");
    }
}
