using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        var tapes = new[]
       {

            new Tape("https://m.media-amazon.com/images/M/MV5BNDg2NjIxMDUyNF5BMl5BanBnXkFtZTgwMzEzNTE1NTM@._V1_SY1000_CR0,0,629,1000_AL_.jpg"),
            new Tape("https://m.media-amazon.com/images/M/MV5BNzAwNzUzNjY4MV5BMl5BanBnXkFtZTgwMTQ5MzM0NjM@._V1_.jpg"),
            new Tape("https://m.media-amazon.com/images/M/MV5BYzEyYzg5N2YtZmYzZC00OTg0LWE3ZmYtNDZhMGFkOTBjOTYxXkEyXkFqcGdeQXVyNDg2MjUxNjM@._V1_SY1000_CR0,0,631,1000_AL_.jpg"),
            new Tape("https://m.media-amazon.com/images/M/MV5BMTc5NzI1NjY4MV5BMl5BanBnXkFtZTgwNDIxNTIyNDM@._V1_SY1000_CR0,0,674,1000_AL_.jpg"),
            new Tape("https://m.media-amazon.com/images/M/MV5BZDVkMWJiMzUtNjQyOS00MGVmLWJhYmMtN2IxYzU4MjY3MDRmXkEyXkFqcGdeQXVyNzA5NjIzODk@._V1_SY1000_CR0,0,675,1000_AL_.jpg"),
            new Tape("https://m.media-amazon.com/images/M/MV5BMDBhOTMxN2UtYjllYS00NWNiLWE1MzAtZjg3NmExODliMDQ0XkEyXkFqcGdeQXVyMjMxOTE0ODA@._V1_SY1000_CR0,0,631,1000_AL_.jpg"),
            new Tape("https://m.media-amazon.com/images/M/MV5BNDg2NjIxMDUyNF5BMl5BanBnXkFtZTgwMzEzNTE1NTM@._V1_SY1000_CR0,0,629,1000_AL_.jpg")
       };
        var rnd = new System.Random();
        var gameTapes = GameObject.FindGameObjectsWithTag("TapeFront");
        var len = tapes.Length - 1;
        foreach (var tape in gameTapes)
        {

            var tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
            var randomNumber = rnd.Next(0, len);
            var randomTape = tapes[randomNumber];
            if (randomTape.Texture != null)
            {
                tape.GetComponent<Renderer>().material.mainTexture = randomTape.Texture;
            }
            else
            {
                using (var www = new WWW(randomTape.URL))
                {
                    yield return www;
                    www.LoadImageIntoTexture(tex);
                    randomTape.Texture = tex;
                    tapes[randomNumber] = randomTape;
                    tape.GetComponent<Renderer>().material.mainTexture = tex;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
