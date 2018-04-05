using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy_0
{
    [Header("Set in Inspector - Enemy_2")]
    public int spacing = 33;
    public float orbitingSpeed = 1f;
    public float orbitRadius = 10f;

    [Header("Set Dynamically - Enemy_2")]
    public GameObject[] minions = new GameObject[4];
    public int numMinions = 0;

    public GameObject particleLaserCharging;
    public GameObject laser;

    private float orb;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine("Charging");
        laser.SetActive(false);
	}

    public override void Move()
    {
        RaycastHit[] raycastHits = Physics.SphereCastAll(pos, 8, Vector3.down);
        switch (status)
        {
            case EnemyState.waiting:
                // move back and forth around the top of the screen...ominously - follow the player
                bool noMove = false;
                for (int k = 0; k < raycastHits.Length; ++k)
                {
                    if (raycastHits[k].collider.gameObject.GetComponent<Enemy>() != null && raycastHits[k].collider.gameObject.GetComponent<Enemy>().rank == ShipRank.enemy_2)
                        noMove = true;
                }

                if (!noMove && Constants.instance.playerPos.x > pos.x) // move right
                    pos = new Vector3(pos.x + (speed / 2 * Time.deltaTime), pos.y, pos.z);
                else if (!noMove)   // move left
                    pos = new Vector3(pos.x - (speed / 2 * Time.deltaTime), pos.y, pos.z);
                break;
            case EnemyState.charging:
                // move the minions
                numMinions = 0;
                for (int i = 0; i < minions.Length; i++)
                {
                    GameObject minion = minions[i];
                    if (minion != null)
                    {
                        // move them minions here
                        orb += Time.deltaTime * orbitingSpeed;
                        Vector3 sqlPos = gameObject.transform.position;
                        minion.GetComponent<Enemy>().pos = new Vector3(sqlPos.x + Mathf.Sin(orb + (spacing * i)) * orbitRadius, 
                                                                        sqlPos.y + Mathf.Cos(orb + (spacing * i)) * orbitRadius, 
                                                                        sqlPos.z);
                        numMinions++;
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
            yield return new WaitForSeconds(5);
            status = EnemyState.charging;
            particleLaserCharging.GetComponent<ParticleSystem>().Play();
            // grab minions
            if (status == EnemyState.in_squad) status = EnemyState.charging;
            RaycastHit[] raycastHits = Physics.SphereCastAll(pos, 50, Vector3.down);
            for (int i = 0; i < minions.Length; ++i)
            {
                if (minions[i] == null)
                {
                    for (int k = 0; k < raycastHits.Length; ++k)
                    {
                        GameObject kGO = raycastHits[k].collider.gameObject;
                        if (kGO.GetComponent<Enemy>() == null) continue;
                        if (!(kGO.GetComponent<Enemy>().rank == ShipRank.enemy_2) && !(kGO.GetComponent<Enemy>().status == EnemyState.in_squad))
                        {
                            minions[i] = raycastHits[k].collider.gameObject;
                            raycastHits[k].collider.gameObject.GetComponent<Enemy>().status = EnemyState.in_squad;
                            ++numMinions;
                            break;
                        }
                    }
                }
            }
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
                minion.GetComponent<Enemy>().status = EnemyState.rushing;
            }
        }
        return base.Dying();
    }
}
