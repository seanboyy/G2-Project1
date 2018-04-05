using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquad : Enemy
{
    [Header("Initialize in Header: EnemySquad")]
    public GameObject squadLeader;
    public int order = 0;        // rank is supposed to be the ordering of the ships orbiting squadLeader. 
    public int spacing = 90;
    public float orbitSpeed = 5f;
    public float orbitRadius = 10f;

    private float orb;

	// Use this for initialization
	void Start ()
    {
		
	}

    public override void Move()
    {
        // Move according to the defualt if the squad leader dies
        // TODO - give different enemy models different movement patterns based if the squad leader dies
        if (squadLeader == null)
        {
            base.Move();
            return;
        }
        if (squadLeader == gameObject)
        {
            speed = 2;
            //base.Move();
            return;
        }
        orb += Time.deltaTime * orbitSpeed;
        Vector3 sqlPos = squadLeader.transform.position;
        pos = new Vector3(sqlPos.x + Mathf.Sin(orb + (spacing * order)) * orbitRadius, sqlPos.y + Mathf.Cos(orb + (spacing * order)) * orbitRadius, sqlPos.z);
    }

}
