using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Color projectileColor;

    [HideInInspector]
    public Rigidbody rb;

    private Renderer rend;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();

        rend.material.color = projectileColor;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
