using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int enemyID;
    public string enemyName;

    public string enemyType;
    public int enemyHP;
    public int enemyATK;
    public float enemySpeed;


    
}
