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

    private void HitPaddle(Vector3 hitPos, GameObject paddle)
    {
        float dist = Vector3.Distance(transform.position, paddle.transform.position);
        Vector3 dir = Vector3.Reflect(direction, hitPos);
        Debug.Log(dir);
        if(dist > 1.75f)
        {
            if(dir.x < 0)
            {
                direction = new Vector3(-0.75f, 0.25f, 0);
            }
            else
            {
                direction = new Vector3(0.75f, 0.25f, 0);
            }
            
        }
        else if(dist < 1.15f)
        {
            if (dir.x < 0)
            {
                direction = new Vector3(-0.25f, 0.75f, 0);
            }
            else
            {
                direction = new Vector3(0.25f, 0.75f, 0);
            }
        }
        else
        {
            if (dir.x < 0)
            {
                direction = new Vector3(-0.5f, 0.5f, 0);
            }
            else
            {
                direction = new Vector3(0.5f, 0.5f, 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string objectTag = collision.gameObject.tag;
        switch(objectTag)
        {
            case "Paddle":
                HitPaddle(collision.contacts[0].normal, collision.gameObject);
                break;
            case "Brick":
                collision.gameObject.GetComponent<BrickClass>().BrickHit();
                HitObject(collision.contacts[0].normal);
                break;
            case "TopWall":
                if(gameManager.gameMode == "Reach")
                {
                    gameManager.BallHitTopWall();
                }
                else
                {
                    HitObject(collision.contacts[0].normal);
                }
                break;
            case "BottomWall":
                gameManager.CheckAllBallsLost(gameObject);
                Destroy(gameObject);
                break;
            default:
                HitObject(collision.contacts[0].normal);
                break;
        }
    }
}
