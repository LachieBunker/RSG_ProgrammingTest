using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMover : MonoBehaviour {

    private float speed = 0.03f;
    private float duration = 1;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject.transform.parent.gameObject, duration);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(0, speed, 0);
	}
}
