using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public Material noGaze;

    public Material gaze;

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material = noGaze;
    }

    public void OnViewed()
    {
        GetComponent<Renderer>().material = gaze;
    }

    public void NoView()
    {
        GetComponent<Renderer>().material = noGaze;
    }
}
