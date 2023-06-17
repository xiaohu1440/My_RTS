using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour,IMovVelocity
{
    [SerializeField]
    private float speed;
    private Vector3 velocity;
    private Rigidbody2D rb;
    private void Awake() 
    {
        rb=GetComponent<Rigidbody2D>();    
    }
    public void SetVelocity(Vector3 velocity)
    {
        this.velocity=velocity;
    }
    private void FixedUpdate()
    {
        rb.velocity=velocity*speed;
    }
}
