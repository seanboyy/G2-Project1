using System.Collections;           // Required for Arrays & other Collections
using System.Collections.Generic;   // Required for Lists and Dictionaries
using UnityEngine;                  // Required for Unity

/// <summary>
/// Enum for each of the states an enemy can be in
/// </summary>
public enum EnemyState
{
    waiting,
    rushing,    // This shall be the swooshing motion we currently have implemented
    attacking   // this will be moving very close to the player and shooting, then return to the waiting position
}


//tentative scores:
//basic enemy: 100 pts
//enemy 1: 125 pts
//enemy 2: 150 pts
//enemy 3: 200 pts
//enemy 4: 500 pts

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;       // The speed in m/s
    public float fireRate = 0.3f;   // Seconds/shot (Unused)
    public float health = 10;
    public int score = 100;         // Points earned for destroying this
    public float showDamageDuration = 0.1f; // # seconds to show damage
    public float powerUpDropChance = 1f;    // Chance to drop a power-up

    public EnemyState status = EnemyState.waiting;

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials;    // All the materials of this & its children
    public bool showingDamage = false;
    public float damageDoneTime;    // Time to stop showing damage
    public bool notifiedOfDestruction = false; 

    protected BoundsCheck bndCheck;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
            originalColors[i] = materials[i].color;
    }

    // This is a Property: A method that acts like a field
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();

        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
	}

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                // If this enemy is off screen, don't damage it
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }

                // Hurt this Enemy
                ShowDamage();
                // Get the damage amound from the Main WEAP_DICT
                //health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                health--;
                if (health <= 0)
                {
                    // Tell the Main singleton that this ship was destroyed
                    if (!notifiedOfDestruction)
                    {
                       //Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    // Destroy this Enemy
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }
    }

    void ShowDamage()
    {
        Debug.Log("Showing Damage");
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;          
    }

    void UnShowDamage()
    {
        Debug.Log("UnShowing Damage");
        for (int i=0; i<materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
