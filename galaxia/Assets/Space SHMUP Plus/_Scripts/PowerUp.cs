﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    // This is an unusual but handy use of Vector2s. x holds a min value 
    //  and y a max value for a Random.Range() that will be called later
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;

    [Header("Set Dynamically")]
    public WeaponType type;         // The type of the PowerUp
    public GameObject cube;         // Reference to the Cube child
    public TextMesh letter;         // Reference to the TextMesh
    public Vector3 rotPerSecond;    // Euler rortation speed
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    void Awake()
    {
        // Find the cube reference
        cube = transform.Find("Cube").gameObject;
        // Find the TextMesh and other components
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        // Set a random velocity
        Vector3 vel = Random.onUnitSphere;  // Get Random XYZ velocity
        // Random.onUnitSphere gives you a vector point that is somewhere on
        //      the surface of the sphere with a radius of 1m around the origin
        vel.z = 0;      // Flatten the vel to the XY plane
        vel.Normalize();    // Normalizing a Vector3 makes it length 1m
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        // Set the rotation of this GameObject to R:[0, 0, 0]
        transform.rotation = Quaternion.identity;
        // Quaternion.identity is equal to no rotation

        // Set up the rotPerSecond for the Cube child using rotMinMax x & y
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        // Fade out the PowerUp over time
        // Given the default values, a PowerUp will exist for 10 seconds
        //  and then fade out over 4 seconds.
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        // For lifeTime seconds, u will be <= 0. Then it will transition to 
        //  1 over the course of fadeTime seconds. 

        // If u >= 1, destroy this PowerUp
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        // Use u to determine the alpha value of the Cube & Letter
        if (u>0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;
            // Fade the Letter too, just not as much
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen)
        {
            // If the PowerUp has drifted entirely off screen, destroy it
            Destroy(gameObject);
        }
	}

    public void SetType(WeaponType wt)
    {
        // Grab the WeaponDefinition from Main
        //WeaponDefinition def = Main.GetWeaponDefinition(wt);
        // Set the color of the Cube child
        //cubeRend.material.color = def.color;
        //letter.color = def.color; // We could colorize the letter too
        //letter.text = def.letter;   // Set the letter that is shown
        type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        // This function is called by the Hero class when a PowerUp is collected
        // We could tween into th target and shrink in size,
        //  but for now, just destroy this.gameObject
        Destroy(this.gameObject);
    }
}
