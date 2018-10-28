using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.InteropServices;
using NFBB.Core;
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
        var menuItem = GameObject.FindGameObjectWithTag("Menu");
        var movieMetaData = gameObject.GetComponentInParent<TapeMetadata>();
        if (movieMetaData)
        {

            var movieId = movieMetaData.MovieMetadata.Id;
            Shopper s = new Shopper();
            var reviews = s.GetReviewsForMovie(movieId);
            var test = menuItem.GetComponent<MenuMetaData>();
            menuItem.GetComponent<MenuMetaData>().Reviews = reviews;
        }

        if (menuItem.GetComponent<MenuMetaData>().Visible)
        {
            menuItem.GetComponent<MenuMetaData>().Hide();
        }
        else
        {
            menuItem.gameObject.GetComponent<MenuMetaData>().Show();
        }
    }
}
