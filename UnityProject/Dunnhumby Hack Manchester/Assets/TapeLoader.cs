using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using NFBB.Core;

public class Tape
{
    public Tape(string url)
    {
        URL = url;
    }
    public Texture2D Texture { get; set; }
    public string URL { get; set; }
}
public class TapeLoader : MonoBehaviour
{

    IEnumerator Start()
    {
        Shopper s = new Shopper();

        var rnd = new System.Random();
        
        var tapeGroups = GameObject.FindGameObjectsWithTag("TapeGroup");
        foreach (var tapeGroup in tapeGroups)
        {
            var genre = tapeGroup.GetComponent<TapeGroup>().GroupGenre;
            var movies = s.PopulateShelvesForGenre(genre).ToList();
            var len = movies.Count;
            var children = tapeGroup.transform.GetComponentsInChildren<TapeMetadata>();
            foreach (var child in children)
            {
                var randomNumber = rnd.Next(0, len);
                var randomMovie = movies[randomNumber];
                var tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
                child.MovieMetadata = randomMovie;
                using (var www = new WWW(randomMovie.PosterUrl))
                {
                    yield return www;
                    www.LoadImageIntoTexture(tex);
                    var frontCover = GetChildObject(child.gameObject.transform, "TapeFront");
                    frontCover.GetComponent<Renderer>().material.mainTexture = tex;
                }
            }
        }
    }
    public GameObject GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                return child.gameObject;
            }
            if (child.childCount > 0)
            {
                GetChildObject(child, _tag);
            }
        }

        return null;
    }
    public float Speed = 1f;
    // Update is called once per frame
    void Update()
    {
        var _velocity = 2f;
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + Camera.main.transform.right * _velocity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + (Camera.main.transform.right * -1) * _velocity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + Camera.main.transform.forward * _velocity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + (Camera.main.transform.forward * -1) * _velocity * Time.deltaTime;
        }

        transform.position = new Vector3(transform.position.x, 30f, transform.position.z);
    }
}
