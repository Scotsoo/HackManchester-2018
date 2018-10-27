using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    Texture2D tex;
	    tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
	    using (WWW www = new WWW("https://www.covercentury.com/covers/dvd/d/deadpool-2016-r1-custom.jpg"))
	    {
	        www.LoadImageIntoTexture(tex);
	        GetComponent<Renderer>().material.mainTexture = tex;
	    }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
