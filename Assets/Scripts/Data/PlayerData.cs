using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int playerID;
    public string playerName;
    public int playerLevel;

    public string playerType;
    public int playerHP;

    public int playerATK;

    public float playerSpeed;



}
