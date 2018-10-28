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
            gameObject.transform.position = new Vector3(-50f, -500f, -500f);
            gameObject.transform.rotation = new Quaternion(0.0f, 0, 0.0f, 0);
            Reviews = new List<Review>();
            tex = null;

        }
    }

    private IEnumerator HandleIt(List<Review> reviews)
    {
        var rnd = new System.Random();
        var randomNumber = rnd.Next(0, reviews.Count - 1);
        var review = reviews[randomNumber];
        var s = new Shopper();
        var user = s.GetUser(review.UserId);
        var moviesSeen = s.GetMoviesSeenByUser(user.UserId);
        var actors = s.GetActorsForMovie(review.MovieId).ToList();
        var actor = actors[rnd.Next(0, actors.Count - 1)];
        var actorCountStat = s.GetMoviesForActor(actor.Name).ToList();
        using (var www = new WWW(user.Image))
        {

            yield return www;
            tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(tex);
            var pic = GameObject.FindGameObjectWithTag("Picture");
            pic.GetComponent<Renderer>().material.mainTexture = tex;
        }
        //var actors = s.get
        var filmCount = GameObject.FindGameObjectWithTag("FilmCount");
        var filmName = GameObject.FindGameObjectWithTag("FilmName");
        var actorCount = GameObject.FindGameObjectWithTag("ActorCount");
        var actorName = GameObject.FindGameObjectWithTag("ActorName");
        actorCount.GetComponent<TextMesh>().text = actorCountStat.Count.ToString();
        actorName.GetComponent<TextMesh>().text = actor.Name;
        filmName.GetComponent<TextMesh>().text = moviesSeen.First(f => f.Id == review.MovieId).Title;
        filmCount.GetComponent<TextMesh>().text = moviesSeen.ToList().Count.ToString();
        var filmNameRating = GameObject.FindGameObjectWithTag("FilmNameRating");
        var randomFilm = moviesSeen.ToList().First(f => f.Id != review.MovieId);
        filmNameRating.GetComponent<TextMesh>().text = randomFilm.Title;
        var userText = GameObject.FindGameObjectWithTag("Fullname");
        userText.GetComponent<TextMesh>().text = user.Name;
        //user.

    }

}

