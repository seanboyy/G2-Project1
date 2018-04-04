using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0 : Enemy
{
    [Header("Set in Inspector: Enemy_3")]
    public float moveSpeed = 5;


    public Vector3 rankPos;
    public bool attacking;
    public float cycleTime;

    // Vectors for attacking. os = off scren, l = left, r = right, d = down, u = up
    private Vector3 oslu, osld, osrd, osru;


    // Use this for initialization
    void Start ()
    {
        rankPos = pos;
        cycleTime = Time.time;
        StartCoroutine("Movement");
        oslu = new Vector3(-30, rankPos.y, rankPos.z);
        osld = new Vector3(-30, -rankPos.y, rankPos.z);
        osrd = new Vector3(rankPos.x, -rankPos.y, rankPos.z);
        osru = new Vector3(30, rankPos.y, rankPos.z);
	}

    public override void Move()
    {

    }

    IEnumerator Movement()
    {
        // this variable is used to keep track of movement cycles for Bezier curves. 
        // TO-DO: refactor variable name; write better comment. 
        float u;
        
        // this should just run through whatever the current movement option is
        while(true)
        {
            switch(status)
            {
                case EnemyState.rushing:    // Enemies approach do not engage with player
                    //Debug.Log("Enemy Status - Rushing");
                    // Bezier curves work based on a u value between 0 & 1
                    u = (Time.time - cycleTime) / moveSpeed;
                    if (u > 1)
                    {
                        status = EnemyState.waiting;
                        break;
                    }
                    // Interpolate the three Bezier curve points
                    Vector3 p01, p12;
                    u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
                    p01 = (1 - u) * rankPos + u * Constants.instance.playerPos;
                    p12 = (1 - u) * Constants.instance.playerPos + u * rankPos;
                    pos = (1 - u) * p01 + u * p12;
                    break;
                case EnemyState.attacking:
                    //Debug.Log("Enemy Status - Attacking");
                    // I want to attack faster
                    u = 2 * (Time.time - cycleTime) / moveSpeed;

                    // Move this gameobject to x = -30
                    if (u < 1)  // this object should have moved to x = -30
                    {
                        pos = ((1 - u) * rankPos) + (u * oslu);
                    }
                    //// Move this gameobject to y = -14
                    //if (u >= 1 && u < 2)
                    //{
                    //    // Makue u < 1
                    //    u = u - 1;
                    //    pos = ((1 - u) * oslu) + (u * osld);
                    //}

                    // Move this gameobject to x = 30
                    if (u >= 1 && u < 2)
                    {
                        // Makue u < 1
                        u = u - 1;
                        pos = ((1 - u) * oslu) + (u * osrd);
                    }

                    // Move this gameobject to y = rankPos.y
                    if (u >= 2 && u < 3)
                    {
                        // Makue u < 1
                        u = u - 2;
                        pos = ((1 - u) * osrd) + (u * osru);
                    }

                    // Move this gameobject to x = rankPos.x
                    if (u >= 3 && u < 4)
                    {
                        // Makue u < 1
                        u = u - 3;
                        pos = ((1 - u) * osru) + (u * rankPos);
                    }
                    if (u >= 4)
                    {
                        status = EnemyState.waiting;
                    }

                    break;
                case EnemyState.waiting:
                default:
                    //Debug.Log("Enemy Status - Waiting (Default)");
                    if (Random.value <= 0.5f)
                        status = EnemyState.rushing;
                    else
                        status = EnemyState.attacking;
                    cycleTime = Time.time;
                    break;
            }
            yield return null;
        }
    }
}
