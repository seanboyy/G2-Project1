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
    in_squad,   // enemy is in a squad and normal movement patterns are ignored. 
    charging,   // this is for enemy_2
    dying       // enemy be deading
}

public enum ShipRank
{
    enemy_0,    // The rank and file                    100 points
    enemy_1,    // The rank and file, can shoot back    125 points
    enemy_2,    // Squad Leaders                        150 points
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

        if (status != EnemyState.dying && health <= 0)
            StartCoroutine("Dying");
	}

    public virtual void Move()
    {
        if (status == EnemyState.dying) return; // do not move if you are dying
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        // do nothing if this enemy is dying
        if (status == EnemyState.dying) return;

        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                //Projectile p = otherGO.GetComponent<Projectile>();
                // If this enemy is off screen, don't damage it
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }
                // Hurt this Enemy
                ShowDamage();
                // This line should be replaced if we allow for damage based on projectile type
                health--;
                Destroy(otherGO);
                break;
            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }
    }

    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;          
    }

    void UnShowDamage()
    {
        for (int i=0; i<materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }

    // This is a coroutine so we can play sounds or whatever if necessary later on
    IEnumerator Dying()
    {
        // notify constants (and anyone else who cares) that this enemy died
        if (status != EnemyState.dying) Messenger<Enemy>.Broadcast(Messages.ENEMY_DESTROYED, this);

        // set the status to dying
        status = EnemyState.dying;
        Instantiate(scoreFloatText).GetComponent<ScoreFloatText>().InitializeText("+" + score, gameObject.transform.position, Color.blue);
        
        // Destroy this Enemy
        Destroy(this.gameObject);

        yield return null;
    }
}