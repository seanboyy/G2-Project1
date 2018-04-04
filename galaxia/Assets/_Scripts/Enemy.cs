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
    attacking,  // this will be moving very close to the player and shooting, then return to the waiting position
    in_squad    // enemy is in a squad and normal movement patterns are ignored. 
}

public enum ShipRank
{
    enemy_0,    // The rank and file                    100 points
    enemy_1,    // The rank and file, can shoot back    125 points
    enemy_2,    // Squad Leaders                        150 points
    enemy_3,    // Not implemented                      200 points
    enemy_4,    // Not implemented                      500 points
    NOT_CLASSIFIED  // default                          0 points
}

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
    public ShipRank rank = ShipRank.NOT_CLASSIFIED;

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials;    // All the materials of this & its children
    public bool showingDamage = false;
    public float damageDoneTime;    // Time to stop showing damage
    public GameObject scoreFloatText;
    public bool notifiedOfDestruction = false;

    protected BoundsCheck bndCheck;
    private int testVal = 0;


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
        // this should just run through whatever the current movement option is
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
                health--;
                if (health <= 0)
                {
                    // Tell the Constants singleton that this enemy was destroyed
                    if (!notifiedOfDestruction)
                    {
                        Debug.Log("Enemy() - Score is " + score);
                        Messenger<Enemy>.Broadcast(Messages.ENEMY_DESTROYED, this);
                        //Debug.Log("TestVal: " + testVal++);
                    }
                    notifiedOfDestruction = true;
                    // Destroy this Enemy
                    Instantiate(scoreFloatText).GetComponent<ScoreFloatText>().InitializeText("+" + score, gameObject.transform.position, Color.blue);
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
