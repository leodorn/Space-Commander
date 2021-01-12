using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>Classe définissant les fonctionnalitées générales d'un ennemi</summary>
public class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] protected float speed = 0,freezeDuration,burnDuration;
    [SerializeField] protected int hp, expDrop;
    protected bool freeze = false,burn = false ;
    protected Rigidbody2D rb;
    public LayerMask actualDimension;
    private float tickBurn = 1f;
    protected List<Animator> enemyAnimators;
    public GameObject redLight;
    protected bool canTakeDamageFromLaser = true;
    [SerializeField]
    bool forceDimension, otherDimension;

    private void Awake() => enemyAnimators = new List<Animator>();


    /// <summary>Gère la dimmension d'un ennemi</summary>
    protected virtual void Start()
    {
        if (forceDimension){
            if (!otherDimension){
                actualDimension = EnemyManager.instance.dimensionALayout;
                for (int i = 0; i < transform.childCount; i++){
                    gameObject.transform.GetChild(i).gameObject.layer = 8;
                }
                gameObject.layer = 8;
            }else{
                for (int i = 0; i < transform.childCount; i++){
                    gameObject.transform.GetChild(i).gameObject.layer = 11;
                }
                actualDimension = EnemyManager.instance.dimensionBLayout;
                gameObject.layer = 11;
            }
        }else{
            int randomDimension = Random.Range(0, 2);
            if (randomDimension == 0){
                actualDimension = EnemyManager.instance.dimensionALayout;
                for (int i = 0; i < transform.childCount; i++){
                    gameObject.transform.GetChild(i).gameObject.layer = 8;
                }
                gameObject.layer = 8;
            }else{
                for (int i = 0; i < transform.childCount; i++) {
                    gameObject.transform.GetChild(i).gameObject.layer = 11;
                }
                actualDimension = EnemyManager.instance.dimensionBLayout;
                gameObject.layer = 11;
            }
        }
       
        
        EnemyManager.instance.enemyList.Add(gameObject);
        EnemyManager.instance.CheckLight(gameObject);
        enemyAnimators.AddRange(GetComponentsInChildren<Animator>());
        rb = GetComponent<Rigidbody2D>();
        
    }

    protected virtual void Update() => RotateToPlayer();


     /// <summary>Oriente l'ennemi vers le joueur</summary>
    protected virtual void RotateToPlayer()
    {
        if (Player.instance != null)
        {
            transform.right = (Vector2)(Player.instance.transform.position - transform.position);
        }
            
    }


    public virtual void TakeDammage(int damage)
    {
        hp -= damage;
        SoundManager.instance.PlayTakeDamagePlayerSound();
        if (enemyAnimators.Count == 1)
            enemyAnimators[0].SetTrigger("Hurt");
        if (hp <= 0)
        {
            Died();
        }
    }


    /// <summary>Deplace l'ennemi vers le joueur</summary>
    protected virtual void FixedUpdate() 
    {
        if(!freeze)
        {
            if(Player.instance !=null)
            {
                Vector2 dir = (Player.instance.transform.position - transform.position).normalized;
                rb.position += dir * speed * Time.fixedDeltaTime;
            }
            
        }
        
    }


    private void Died()
    {
        ParticuleManagerScript.instance.CreateExplosion(transform.position);
        SliderManager.instance.GainExp(expDrop);
        EnemyManager.instance.enemyList.Remove(gameObject);
        SoundManager.instance.PlayExplosionEnemie();
        Destroy(gameObject);
    }


    public void Freeze() => StartCoroutine(FreezeTimer());

    public void Burn()
    {
        if(!burn)
        {
            ColorRedEnemy();
            burn = true;
            StartCoroutine(BurnTimer(burnDuration));
            
        }
        
    }

    /// <summary>Brule l'ennemi et lui inflige des dégât à interval régulié</summary>
    public IEnumerator BurnTimer(float burnDurationLeft)
    {
        yield return new WaitForSeconds(tickBurn);
        burnDurationLeft -= tickBurn;
        if(burnDurationLeft > 0)
        {
            StartCoroutine(BurnTimer(burnDurationLeft));
            this.TakeDammage(1);
        }
        else
        {
            ResetColorEnemy();
            burn = false;
        }
    }
    /// <summary>Gèle l'ennemi</summary>
    public IEnumerator FreezeTimer()
    {
        ColorBlueEnemy();
        freeze = true;
        yield return new WaitForSeconds(freezeDuration);
        ResetColorEnemy();
        freeze = false;
    }


    public void ResetColorEnemy()
    {
        for(int child = 0; child < transform.childCount;child++)
        {
            transform.GetChild(child).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void ColorRedEnemy()
    {
        for (int child = 0; child < transform.childCount; child++)
        {
            transform.GetChild(child).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void ColorBlueEnemy()
    {
        for (int child = 0; child < transform.childCount; child++)
        {
            transform.GetChild(child).GetComponent<SpriteRenderer>().color =  new Color(0, 54, 255);
        }
    }

    public virtual void AddHp(int hp) => this.hp += hp;


    /// <summary>Inflige des dégats de collision au joueur</summary>
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            Player.instance.TakeDamage(1);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Laser"))
        {
            TakeLaser();
        }
    }

    /// <summary>Prend les dégâts du laser du joueur</summary>
    public void TakeLaser()
    {
        if(canTakeDamageFromLaser)
        {
            StartCoroutine(LaserTimer());
            TakeDammage(1+Player.instance.damageBonus);
        }
    }

    public IEnumerator LaserTimer()
    {
        canTakeDamageFromLaser = false;
        yield return new WaitForSeconds(0.3f);
        canTakeDamageFromLaser = true;
    }

    
}
