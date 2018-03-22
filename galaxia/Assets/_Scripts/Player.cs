using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float movement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector3 pos = gameObject.transform.position;
        gameObject.transform.position = new Vector3(pos.x + movement, pos.y, pos.z);
	}
}
