using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float rotationSpeed = 10F;
    public float rollMult = -45;
    public float pitchMult = 30;

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
        Vector2 movement;
        if (!isRolling)
        {
            movement.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            movement.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        }
        else
        {
            movement = Vector2.zero;
        }
        Vector3 pos = gameObject.transform.position;
        gameObject.transform.position = new Vector3(pos.x + movement.x, pos.y + movement.y, pos.z);

        // Rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(movement.y * pitchMult, movement.x * rollMult, 0);

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

        // Update constants so enemies can hunt down the player. 
        Constants.instance.playerPos = transform.position;
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

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

    }
}
