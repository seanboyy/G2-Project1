using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0 : Enemy
{
    [Header("Set in Inspector: Enemy_3")]
    public float lifeTime = 5;


    public Vector3 rankPos;
    public bool attacking;
    public float cycleTime;


    // Use this for initialization
    void Start ()
    {
        rankPos = pos;
        cycleTime = Time.time;
	}

    public override void Move()
    {
        if (attacking)
        {
            // Bezier curves work based on a u value between 0 & 1
            float u = (Time.time - cycleTime) / lifeTime;
            if (u > 1)
            {
                attacking = false;
                return;
            }

            // Interpolate the three Bezier curve points
            Vector3 p01, p12;
            u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
            p01 = (1 - u) * rankPos + u * Constants.instance.playerPos;
            p12 = (1 - u) * Constants.instance.playerPos + u * rankPos;
            pos = (1 - u) * p01 + u * p12;
        }
        else if (Random.value > 0.5f)
        {
            attacking = true;
            cycleTime = Time.time;
        }
    }
}
