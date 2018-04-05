using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy_0
{
    [Header("Set Dynamically - Enemy_2")]
    public GameObject[] minions = new GameObject[4];
    public int numMinions = 0;

    public GameObject particleLaserCharging;
    public GameObject laser;

	// Use this for initialization
	void Start ()
    {
        laser.SetActive(false);
        StartCoroutine("Charging");
	}

    public override void Move()
    {
        switch(status)
        {
            case EnemyState.waiting:
                // move back and forth around the top of the screen...ominously - follow the player
                if (Constants.instance.playerPos.x > pos.x) // move right
                    pos = new Vector3(pos.x + (speed / 2 * Time.deltaTime), pos.y, pos.z);
                else    // move left
                    pos = new Vector3(pos.x - (speed / 2 * Time.deltaTime), pos.y, pos.z);
                //pos = new Vector3(Constants.instance.playerPos.x, pos.y, pos.z);
                break;
            //case EnemyState.charging:
            //    // grab nearby enemies and turn them into minions
            //    break;
            //case EnemyState.attacking:
            //    // fire a massive laser
            //    break;
            default:
                // do nothing
                break;
        }
    }

    IEnumerator Charging()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            status = EnemyState.charging;
            particleLaserCharging.GetComponent<ParticleSystem>().Play();
            // grab minions
            yield return new WaitForSeconds(5);
            // fire laser
            particleLaserCharging.GetComponent<ParticleSystem>().Stop();
            laser.SetActive(true);
            laser.transform.localScale = new Vector3(1 + numMinions, laser.transform.localScale.y, laser.transform.localScale.z);
            yield return new WaitForSeconds(5);
            laser.SetActive(false);
            status = EnemyState.waiting;
        }
    }

}
