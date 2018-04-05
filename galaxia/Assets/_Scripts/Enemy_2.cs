using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy_0
{
    [Header("Set Dynamically - Enemy_2")]
    public GameObject[] minions = new GameObject[4];

	// Use this for initialization
	void Start ()
    {
		
	}

    public override void Move()
    {
        switch(status)
        {
            case EnemyState.waiting:
                // move back and forth around the top of the screen...ominously - follow the player
                pos = new Vector3(Constants.instance.playerPos.x, pos.y, pos.z);
                break;
            case EnemyState.charging:
                // grab nearby enemies and turn them into minions
                break;
            case EnemyState.attacking:
                // fire a massive laser
                break;
            default:
                // do nothing
                break;
        }
    }

}
