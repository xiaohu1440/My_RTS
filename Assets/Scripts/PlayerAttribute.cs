using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public PlayerData playerData;

    [HideInInspector]
    public int playerID;
    public string playerName;
    [HideInInspector]
    public int playerLevel;
    [HideInInspector]

    public string playerType;
   
    public int playerHP;
    [HideInInspector]

    public int playerATK;
    [HideInInspector]

    public float playerSpeed;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        playerID = playerData.playerID;
        playerName = playerData.playerName;
        playerLevel = playerData.playerLevel;
        playerType = playerData.playerType;
        playerHP = playerData.playerHP;
        playerATK = playerData.playerATK;
        playerSpeed = playerData.playerSpeed;
    }
}
