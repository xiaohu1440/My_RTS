using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionDirect : MonoBehaviour
{
    private Vector3 movePosition;
    private void Awake() 
    {
        movePosition = transform.position;
    }

    public void SetMovePosition(Vector3 position)
    {
        this.movePosition = position; 
    }

    private void Update() 
    {
        Vector3 moveDir=(movePosition-transform.position).normalized;
        if(Vector3.Distance(transform.position,movePosition)<1f)
        {
            moveDir=Vector3.zero;
            GetComponent<IMovVelocity>().SetVelocity(moveDir);
        }
        
        
    }

   
}
