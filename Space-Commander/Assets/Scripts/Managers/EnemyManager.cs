using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public LayerMask dimensionALayout, dimensionBLayout;

    public List<GameObject> enemyList;

    private void Awake()
    {
        if(instance == null){
            instance = this;
        }
    }

    public void CheckLight(GameObject enemy)
    {
        BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
        if (baseEnemy.actualDimension != Player.instance.actualDimension){
            baseEnemy.redLight.SetActive(true);
        }else{
            baseEnemy.redLight.SetActive(false);
        }
    }


    public void ChangeDimensionEnemy()
    {
        foreach (GameObject enemy in enemyList){
            CheckLight(enemy);
        }
    }
}
