using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using NFBB.Core;
using NFBB.Core.Data;
using UnityEngine;

public class MenuMetaData : MonoBehaviour
{
    public bool Visible = false;
    public IEnumerable<Review> Reviews = new List<Review>();
    public Texture2D tex = null;
    public void Show()
    {
        Visible = true;
    }

    public void Hide()
    {
        Visible = false;
        Reviews = new List<Review>();
        tex = null;
    }
	// Use this for initialization
	void Start () {
		Hide();
	}

    // Update is called once per frame
    public float distance = 5;

    void Update()
    { 
        if(Visible)
        {
            var reviews = Reviews.ToList();
            if (reviews.Count > 1 && tex == null) {
                StartCoroutine(HandleIt(reviews));

            }
            gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
            gameObject.transform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);
        }
        else
        {
            gameObject.transform.position = new Vector3(0f, 0f, 0f);
            gameObject.transform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);

        }
    }

    private IEnumerator HandleIt(List<Review> reviews)
    {
        var rnd = new System.Random();
        var randomNumber = rnd.Next(0, reviews.Count - 1);
        var review = reviews[randomNumber];
        var s = new Shopper();
        var user = s.GetUser(review.UserId);

        using (var www = new WWW(user.Image))
        {

            yield return www;
            tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(tex);
            var pic = GameObject.FindGameObjectWithTag("Picture");
            pic.GetComponent<Renderer>().material.mainTexture = tex;
        }
        }

    }

