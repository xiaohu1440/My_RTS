using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class AsterMove : MonoBehaviour,IMovePosition
{
   private AIPath aiPath;
   private void Awake()
    {
        aiPath = GetComponent<AIPath>();
    }
    public void SetMovePosition(Vector3 position, Action onReachedMovePosition)
    {
        aiPath.destination = position;
        
    }
}
