using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {

    public float speed;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update () {
        if(Input.mousePosition.x > 0.0f && Input.mousePosition.x < Screen.width && gameManager.gameState == GameState.Playing)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).x, speed), transform.position.y, transform.position.z);
        }
	}
}
