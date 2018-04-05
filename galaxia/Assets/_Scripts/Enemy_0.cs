using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0 : Enemy
{
    [Header("Set Dynamically: Enemy_0")]
    public Vector3 rankPos;
    public bool attacking;
    public float cycleTime;

    // Vectors for attacking. 
    protected Vector3 upperOffScreenLeft, lowerRankPos, upperOffScreenRight, rushPos;


    // Use this for initialization
    void Start ()
    {
        // start the first movement cycle
        cycleTime = Time.time;
        // correctly assign the ship's rank
        rank = ShipRank.enemy_0;

        // Set up positioning things
        rankPos = pos;
        upperOffScreenLeft = new Vector3(-30, rankPos.y, rankPos.z);
        lowerRankPos = new Vector3(rankPos.x, -rankPos.y, rankPos.z);
        upperOffScreenRight = new Vector3(30, rankPos.y, rankPos.z);
    }

    public override void Move()
    {
        // this variable is used to keep track of movement cycles for Bezier curves. 
        // TO-DO: refactor variable name; write better comment. 
        float u;

        switch (status)
        {
            case EnemyState.rushing:    // Enemies approach do not engage with player
                //Debug.Log("Enemy Status - Rushing");
                // Bezier curves work based on a u value between 0 & 1
                u = (Time.time - cycleTime) / speed;
                if (u > 1)
                {
                    status = EnemyState.waiting;
                    break;
                }
                // Interpolate the three Bezier curve points
                Vector3 p01, p12;
                u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
                p01 = (1 - u) * rankPos + u * rushPos;
                p12 = (1 - u) * rushPos + u * rankPos;
                pos = (1 - u) * p01 + u * p12;
                break;
            case EnemyState.attacking:
                //Debug.Log("Enemy Status - Attacking");
                u = 4 * (Time.time - cycleTime) / speed;
                // Move enemy off screen left
                if (u < 1)
                {
                    pos = ((1 - u) * rankPos) + (u * upperOffScreenLeft);
                }
                // Move enemy to the opposite of it's rankPos
                if (u >= 1 && u < 2)
                {
                    // Makue u < 1
                    u = u - 1;
                    pos = ((1 - u) * upperOffScreenLeft) + (u * lowerRankPos);
                }
                // Move enemy off screen right
                if (u >= 2 && u < 3)
                {
                    // Makue u < 1
                    u = u - 2;
                    pos = ((1 - u) * lowerRankPos) + (u * upperOffScreenRight);
                }
                // Return to rankPos
                if (u >= 3 && u < 4)
                {
                    // Makue u < 1
                    u = u - 3;
                    pos = ((1 - u) * upperOffScreenRight) + (u * rankPos);
                }
                if (u >= 4)
                {
                    status = EnemyState.waiting;
                }

                break;
            case EnemyState.in_squad:
                Debug.Log("Not Implemented");
                break;
            case EnemyState.waiting:
                if (Random.value <= 0.5f)
                {
                    status = EnemyState.rushing;
                    rushPos = Constants.instance.playerPos;
                }
                else
                    status = EnemyState.attacking;
                cycleTime = Time.time;
                break;
            default:
                base.Move();
                break;
        }
    }
}