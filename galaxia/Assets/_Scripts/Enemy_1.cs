using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy_0
{

    public GameObject projectilePrefab;
    public float projectileSpeed;

    // Use this for initialization
    void Start()
    {
        cycleTime = Time.time;
        rank = ShipRank.enemy_1;
    }

    public override void Move()
    {
        switch (status)
        {
            case EnemyState.attacking:
                if (!firing)
                {
                    firing = true;
                    int times = Random.Range(2, 5);
                    StartCoroutine(Fire(times));
                }
                Debug.Log("Moving into Enemy_0.Move() : status = attacking");
                base.Move();
                break;
            default:
                firing = false;
                base.Move();
                break;
        }
    }

    IEnumerator Fire(int times)
    {
        for (int i = 0; i < times; ++i)
        {
            Instantiate(projectilePrefab, gameObject.transform.position, new Quaternion()).GetComponent<Rigidbody>().velocity = Vector3.down * projectileSpeed;
            yield return new WaitForSeconds(fireRate);
        }
        cycleTime = Time.time;
        yield return null;
    }
}
