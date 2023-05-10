using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerFog : MonoBehaviour
{

    private bool isPaused = true;
    private float timeElapsed = 0f;
    private int CricleFog=3;
    public Tilemap fogOfWar;
    public float timeFog=1f;


    private void OnTriggerEnter2D(Collider2D other)
     {
        Debug.Log("Player is in the trigger");
        if (other.gameObject.tag == "Player")
        
        {
            Debug.Log("Player is in the trigger");
            
            isPaused = false;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            isPaused = true;
        }
    }

    private void Update()
    {
        if (!isPaused)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeFog)
            {
                CricleFog++;
                timeElapsed = 0f;
                
            }
            
        }
        UpdateFog();
        
    }
    private void UpdateFog()
    {
        Vector3Int currentTower=fogOfWar.WorldToCell(transform.position);
        for(int i=-CricleFog;i<=CricleFog;i++)
        {
            for(int j=-CricleFog;j<=CricleFog;j++)
            {
                fogOfWar.SetTile(currentTower+new Vector3Int(i,j,0) ,null);
            }
        }
    }
}
