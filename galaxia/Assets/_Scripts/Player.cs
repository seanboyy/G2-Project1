using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float rotationSpeed = 10F;
    
    private bool isRolling = false; 
    
    [Header("Shooting")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	void Update ()
    {
        float movement;
        if (!isRolling)
        {
            movement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        }
        else
        {
            movement = 0;
        }
        Vector3 pos = gameObject.transform.position;
        gameObject.transform.position = new Vector3(pos.x + movement, pos.y, pos.z);
        if (Input.GetKeyDown(KeyCode.Q) && !isRolling)
        {
            isRolling = true;
            StartCoroutine("BarrelRollLeft");
        }
        if (Input.GetKeyDown(KeyCode.E) && !isRolling)
        {
            isRolling = true;
            StartCoroutine("BarrelRollRight");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate<GameObject>(projectilePrefab, gameObject.transform.position, new Quaternion()).GetComponent<Rigidbody>().velocity = Vector3.up * projectileSpeed;
        }
	}

    IEnumerator BarrelRollLeft()
    {
        while(transform.rotation.eulerAngles.y < 349)
        {
            transform.Rotate(Vector3.up * rotationSpeed);
            transform.position += Vector3.left * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        isRolling = false;
        StopCoroutine("BarrelRollLeft");
    }

    IEnumerator BarrelRollRight()
    {
        do
        {
            transform.Rotate(Vector3.down * rotationSpeed);
            transform.position += Vector3.right * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (transform.rotation.eulerAngles.y > 11);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        isRolling = false;
        StopCoroutine("BarrelRollRight");
    }
}
