using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed=0.05f;
    private Transform startPoint;
    private Vector2 endPoint;
    // Start is called before the first frame update
    void Start()
    {
        startPoint=this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            
            endPoint=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(endPoint);
            MovePlayer();
        }
    }


    

    private void MovePlayer()
        {
            
            Debug.Log("MovePlayer");
            float x=Mathf.Round(transform.position.x- endPoint.x);
            float y=Mathf.Round(transform.position.y- endPoint.y);
            endPoint=new Vector2(transform.position.x-x,transform.position.y-y);
            //玩家移动x，y轴的距离
            
            Debug.Log(new Vector2(transform.position.x-x,transform.position.y-y));
            transform.position=Vector2.MoveTowards(transform.position,endPoint,speed);
            

        }
    }
