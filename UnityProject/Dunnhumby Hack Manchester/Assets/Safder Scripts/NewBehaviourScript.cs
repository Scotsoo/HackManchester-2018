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
        Debug.Log("ON VIEW!!");
        GetComponent<Renderer>().material = gaze;
    }

    public void OnTap()
    {
        Debug.Log("TAP!!");
    }

    public void NoView()
    {
        Debug.Log("ON noview!!");

        GetComponent<Renderer>().material = noGaze;
    }
}
