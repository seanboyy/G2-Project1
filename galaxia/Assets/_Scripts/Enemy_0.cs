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


    // Use this for initialization
    void Start ()
    {
        rankPos = pos;
        cycleTime = Time.time;
        StartCoroutine("Movement");
	}

    public override void Move()
    {
        //if (status == EnemyState.attacking)
        //{
        //    // Bezier curves work based on a u value between 0 & 1
        //    float u = (Time.time - cycleTime) / lifeTime;
        //    if (u > 1)
        //    {
        //        status = EnemyState.waiting;
        //        return;
        //    }

        //    // Interpolate the three Bezier curve points
        //    Vector3 p01, p12;
        //    u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
        //    p01 = (1 - u) * rankPos + u * Constants.instance.playerPos;
        //    p12 = (1 - u) * Constants.instance.playerPos + u * rankPos;
        //    pos = (1 - u) * p01 + u * p12;
        //}
        //else if (Random.value > 0.5f)
        //{
        //    status = EnemyState.attacking;
        //    cycleTime = Time.time;
        //}
    }

    IEnumerator Movement()
    {
        // this should just run through whatever the current movement option is
        while(true)
        {
            switch(status)
            {
                case EnemyState.rushing:    // Enemies approach do not engage with player
                    //Debug.Log("Enemy Status - Rushing");
                    // Bezier curves work based on a u value between 0 & 1
                    float u = (Time.time - cycleTime) / moveSpeed;
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
                    // Move this gameobject to x = -30

                    // Move this gameobject to y = -14
                    // Move this gameobject to x = 30
                    // Move this gameobject to y = rankPos.y
                    // Move this gameobject to x = rankPos.x
                    break;
                case EnemyState.waiting:
                default:
                    //Debug.Log("Enemy Status - Waiting (Default)");
                    status = EnemyState.rushing;
                    cycleTime = Time.time;
                    break;
            }
            yield return null;
        }
    }
}
