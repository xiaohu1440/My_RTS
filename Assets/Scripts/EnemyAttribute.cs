using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttribute : MonoBehaviour
{
    public EnemyData enemyData;

    [HideInInspector]
    public int enemyID;
    public string enemyName;

    [HideInInspector]
    public string enemyType;

    public int enemyHP;
    [HideInInspector]
    public int enemyATK;
    [HideInInspector]
    public float enemySpeed;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        enemyID = enemyData.enemyID;
        enemyName = enemyData.enemyName;
        enemyType = enemyData.enemyType;
        enemyHP = enemyData.enemyHP;
        enemyATK = enemyData.enemyATK;
        enemySpeed = enemyData.enemySpeed;
    }

   
}
