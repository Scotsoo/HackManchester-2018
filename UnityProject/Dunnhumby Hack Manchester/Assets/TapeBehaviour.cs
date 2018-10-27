using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeBehaviour : MonoBehaviour {

    // Use this for initialization
    IEnumerator Start()
    {
        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        using (WWW www = new WWW("https://m.media-amazon.com/images/M/MV5BNDg2NjIxMDUyNF5BMl5BanBnXkFtZTgwMzEzNTE1NTM@._V1_SY1000_CR0,0,629,1000_AL_.jpg"))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
