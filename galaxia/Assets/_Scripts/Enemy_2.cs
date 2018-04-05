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
                break;
            case EnemyState.charging:
                //grab minions
                for(int i = 0; i < minions.Length; ++i)
                {
                    if(minions[i] == null)
                    {
                        if (numMinions > 0) --numMinions;
                        RaycastHit[] raycastHits = Physics.SphereCastAll(pos, 50F, Vector3.down);
                        for (int k = 0; k < raycastHits.Length; ++k)
                        {
                            if (!raycastHits[k].collider.gameObject.name.Contains("Enemy_2") && raycastHits[k].collider.gameObject.tag == "Enemy" && !raycastHits[k].collider.gameObject.GetComponent<Enemy>().isMinion)
                            {
                                minions[i] = raycastHits[k].collider.gameObject;
                                raycastHits[k].collider.gameObject.GetComponent<Enemy>().isMinion = true;
                                raycastHits[k].collider.gameObject.GetComponent<Enemy>().masterPos = pos;
                                ++numMinions;
                                break;
                            }
                        }
                    }
                }
                break;
            case EnemyState.attacking:
                StartCoroutine("Charging");
                break;
            default:
                // do nothing
                break;
        }
    }

    IEnumerator Charging()
    {
        while (true)
        {
            if (isMinion) isMinion = false;
            status = EnemyState.charging;
            yield return new WaitForSeconds(5);
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

    protected override IEnumerator Dying()
    {
        foreach(GameObject minion in minions)
        {
            if (minion != null)
            {
                minion.GetComponent<Enemy>().isMinion = false;
                minion.GetComponent<Enemy>().status = EnemyState.rushing;
            }
        }
        return base.Dying();
    }
}
