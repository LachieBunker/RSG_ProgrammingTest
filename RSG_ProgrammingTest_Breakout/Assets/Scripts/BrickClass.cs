using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickClass : MonoBehaviour {

    public Color colour;
    public int score;

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = colour;
    }

    //Brick was hit by ball
    public void BrickHit()
    {
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().UpdateScore(score);
        Destroy(gameObject);
    }
}
