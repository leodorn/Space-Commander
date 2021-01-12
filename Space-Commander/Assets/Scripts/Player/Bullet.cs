using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Vector2 target;
    [SerializeField]
    float speedBullet;
    [SerializeField]
    int damage;
    [SerializeField]
    float lifeTime;
    [SerializeField]
    Sprite normalBulletSprite, fireBulletSprite, iceBulletSprite;
    bool ice, fire;



    /// <summary>Appelée lors de la création d'une bullet</summary>
    public void InitializeBullet(Vector2 target,bool ice,bool fire)
    {
        SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.ice = ice;
        this.fire = fire;
        if(ice){
            spriteRenderer.sprite = iceBulletSprite;
        }else if(fire){
            spriteRenderer.sprite = fireBulletSprite;
        }else{
            spriteRenderer.sprite = normalBulletSprite;
        }

        SetTarget(target);
    }

    
    /// <summary>Tourne la bullet vers sa cible et commence le chrono de la durée de vie de la bullet</summary>
    public void SetTarget(Vector2 target)
    {
        this.target = target;
        float angle = Mathf.Atan2(-target.x, target.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.GetChild(0).rotation = rotation;
        StartCoroutine(LifeTime(lifeTime));
        
    }


    ///<summary>Durée de vie de la bullet </summary>
    private IEnumerator LifeTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }


    ///<summary>Deplace la bullet vers sa cible </summary>
    private void FixedUpdate()
    {
        transform.Translate(target.normalized * speedBullet * Time.deltaTime);
    }


    ///<summary> Détruit la bullet lors d'une collision et altère l'état d'un ennemi <summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy")){
            if(ice){
                    collision.gameObject.GetComponent<IEnemy>().Freeze();
            }if(fire){
                collision.gameObject.GetComponent<IEnemy>().Burn();
            }
            collision.gameObject.GetComponent<IEnemy>().TakeDammage(damage+ Player.instance.damageBonus);
        }
        Destroy(gameObject); 
    }
}
