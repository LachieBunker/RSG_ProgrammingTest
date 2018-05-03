using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrickClass : MonoBehaviour {

    public Color colour;
    public int score;
    public GameObject uiNotification;

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = colour;
    }

    //Brick was hit by ball
    public void BrickHit()
    {
        GameManager gManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        gManager.UpdateScore(score);
        if(gManager.gameMode == "Clear")
        {
            gManager.CheckAllBricksDestroyed(gameObject);
        }
        //Spawn UI notification when brick destroyed
        GameObject temp = (GameObject)Instantiate(uiNotification, transform.position, Quaternion.identity);
        temp.transform.localScale = new Vector3(0.05f, 0.05f, 0.50f);
        temp.transform.GetChild(0).transform.position = new Vector3(transform.position.x, transform.position.y, -2);
        temp.GetComponentInChildren<Text>().text = "+" + score.ToString();
        Destroy(gameObject);
    }
}
