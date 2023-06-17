using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttribute : MonoBehaviour
{
    public TowerData towerData;
    public int towerID;
    public float towerTime;
    public float towerHP;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        towerID = towerData.towerID;
        towerTime = towerData.towerTime;
        towerHP = towerData.towerHP;
    }
}
