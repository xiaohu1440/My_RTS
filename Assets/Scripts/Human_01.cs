using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_01 : MonoBehaviour
{
    private void OnMouseDown() 
    {
        PlayerMovement.instance=GetComponent<PlayerMovement>();
    }
}
