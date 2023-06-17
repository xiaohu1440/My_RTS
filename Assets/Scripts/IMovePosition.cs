using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface IMovePosition
{
    void SetMovePosition(Vector3 position,Action onReachedMovePosition);
}