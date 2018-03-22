using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 10F;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine("BarrelRoll");
        }
	}

    IEnumerator BarrelRoll()
    {
        while(transform.rotation.eulerAngles.y < 349)
        {
            transform.Rotate(Vector3.up * rotationSpeed);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        StopCoroutine("BarrelRoll");
    }
}
