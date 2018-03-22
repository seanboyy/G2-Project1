using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float rotationSpeed = 10F;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate<GameObject>(projectilePrefab, gameObject.transform.position, new Quaternion()).GetComponent<Rigidbody>().velocity = Vector3.up * projectileSpeed;
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
