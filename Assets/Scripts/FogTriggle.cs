using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTriggle : MonoBehaviour
{
   
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyMove>().isFog=true;
        }
    }
}
