using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    private Vector3 direction;
    private float speed;
    private GameManager gameManager;

	// Use this for initialization
	void Start ()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(gameManager.gameState == GameState.Playing)
        {
            transform.Translate(direction * speed);
        }
	}

    public void SetProperties(Vector3 dir, float _speed)
    {
        direction = dir;
        speed = _speed;
    }

    //Bounce when the ball hits an object
    private void HitObject(Vector3 whatIHit)
    {
        direction = Vector3.Reflect(direction, whatIHit);
    }

    private void OnCollisionEnter(Collision collision)
    {
        string objectTag = collision.gameObject.tag;
        switch(objectTag)
        {
            case "Paddle":
                HitObject(collision.contacts[0].normal);
                break;
            case "Brick":
                collision.gameObject.GetComponent<BrickClass>().BrickHit();
                HitObject(collision.contacts[0].normal);
                break;
            case "TopWall":
                gameManager.BallHitTopWall();
                break;
            case "BottomWall":
                gameManager.BallLost();
                break;
            default:
                HitObject(collision.contacts[0].normal);
                break;
        }
    }
}
