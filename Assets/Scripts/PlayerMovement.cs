using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public TileBase borderTile;
    public TileBase originalTile;

    public float moveSpeed = 2f;
    //public NavMeshAgent agent;
    public Tilemap tilemap;
    private Vector3Int PlayerTran;
    public TileBase redTile;
    private Vector3Int currentCell;
    private bool isSelected=false;
    private Vector3Int targetCell;
    public GameObject click;
    public Vector3Int TargetCell 
    { 
        get => targetCell; 
        set 
        {
            
            targetCell = value;
            if (calculatePathCoroutine != null)
            {
                StopCoroutine(calculatePathCoroutine);
            }
            calculatePathCoroutine = StartCoroutine(CalculatePath(currentCell, targetCell));
            calculatePathCoroutine = null;
        } 
    }
 

    private Vector3Int[] offset;
    [SerializeField]
    private List<Vector3Int> path;
    private int currentPathIndex;
    private bool isFog=false;
    private bool isMoving;
    private Coroutine calculatePathCoroutine;

    private void Awake() {
        
    }
    private void Start()
    {
       
        //Debug.Log("开始");
        offset=new Vector3Int[]{new Vector3Int(1,0,0),new Vector3Int(-1,0,0),new Vector3Int(0,1,0),new Vector3Int(0,-1,0)};
        currentCell = tilemap.WorldToCell(transform.position);
        TargetCell = currentCell;
        //Debug.Log("当前坐标"+currentCell);
        path = new List<Vector3Int>();
        isMoving = false;
       
    }

    
    private void Update()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
            
        if(instance==this)
        {
            if (hit1.collider!=null)
            {
                GameObject hitObject = hit1.collider.gameObject;
                string hitObjectTag = hitObject.tag;
                if(hitObjectTag=="Fog")
                {
                    
                    isFog=true;
                    
                }
                
                
            }
            else
                {
                    
                    isFog=false;
                    
                }
        PlayerTran=tilemap.WorldToCell(transform.position);
        
        if(isSelected)
        {
            //ChangeTiles(borderTile);
            Time.timeScale=0.2f;
        }
        else
        {
            //ChangeTiles(originalTile);
            Time.timeScale=1f;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
            
            if (hit.collider!=null)
            {
                GameObject hitObject = hit.collider.gameObject;
                string hitObjectTag = hitObject.tag;
                if(hitObjectTag=="Player")
                {
                    click.SetActive(true);
                    isSelected=true;
                    //Debug.Log("是否选中"+isSelected);
                }
                
                
            }
            else
                {
                    
                    isSelected=false;
                    click.SetActive(false);
                    //Debug.Log("更改后是否选中"+isSelected);
                }
            
           
            
        }
        if(Input.GetMouseButtonDown(1)&&isSelected&&isFog==false) 
           {
            
            path.Clear();
            currentCell = PlayerTran;
            Vector3 clickPosition = tilemap.CellToWorld(tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            Debug.Log("点击坐标1"+clickPosition);
            Vector3Int clickCell = tilemap.WorldToCell(clickPosition);
            Debug.Log("点击坐标2"+clickCell);
            clickCell.z = 0;
            //Debug.Log("点击坐标"+clickCell);
            //Debug.Log("当前坐标"+TargetCell);
                if (clickCell != TargetCell)
                {
                    //Debug.Log("panduan");
                    TargetCell = clickCell;
                    
                    
                    //Debug.Log("路径长度"+path.Count);
                    currentPathIndex = 0;
                    isMoving = true;
                    //Debug.Log(isMoving);
                }
            isSelected=false;
            click.SetActive(false);
                
            }
        
        if (isMoving)
        {
            
            Vector3Int nextCell = path[currentPathIndex];
            
            //Debug.Log("比较"+PlayerTran+nextCell);
            if (PlayerTran == nextCell)
            {
                
                currentCell = nextCell;
                currentPathIndex++;

                if (currentPathIndex >= path.Count)
                {
                    isMoving = false;
                    return;
                }

                
                nextCell = path[currentPathIndex];
                //Debug.Log("下一节点"+nextCell);
            }
            
            Vector3 moveDirection = (tilemap.GetCellCenterWorld(nextCell) - transform.position).normalized;
            
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
        }
    }

    public void BreadFrist(Vector3Int currentCell, Vector3Int TargetCell)
    {
        StartCoroutine(CalculatePath(currentCell, TargetCell)); 
    }

    public int Manhandun(Vector3Int a,Vector3Int b)
    {
        return Mathf.Abs(a.x-b.x)+Mathf.Abs(a.y-b.y);
    }
   

    IEnumerator CalculatePath(Vector3Int startCell, Vector3Int TargetCell)
    {
        
        Debug.Log("尺寸x"+tilemap.cellBounds.size.x+"尺寸y"+tilemap.cellBounds.size.y);
        List<Vector3Int> playerPath = new List<Vector3Int>();
        //Queue<Vector3Int> queue = new Queue<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        //queue.Enqueue(startCell);
        playerPath.Add(startCell);
        cameFrom[startCell] = new Vector3Int(tilemap.cellBounds.size.x, tilemap.cellBounds.size.y, TargetCell.z);
        
        while(playerPath.Count>0)
        {
            
            //Debug.Log("队列长度"+playerPath.Count);
            playerPath.Sort((Vector3Int a,Vector3Int b)=>{
            return (Manhandun(startCell,a)+Manhandun(TargetCell,a))-(Manhandun(startCell,b)+Manhandun(TargetCell,b));
            });
            
            //Vector3Int current = queue.Dequeue();
            Vector3Int current=playerPath[0];
            playerPath.RemoveAt(0);//移除第一个元素
            Debug.Log("当前坐标+目标点"+current+TargetCell);
            
            if(current==TargetCell)
            {
                //Debug.Log("到达目标点");
                break;
            }
            List<Vector3Int> newPositions = new List<Vector3Int>();
            foreach(Vector3Int direction in offset)
            {
                
                Vector3Int newPos = current + direction;
               //Debug.Log("当前tilemap名称"+tilemap.GetTile(newPos).name=="Ground");
                //Debug.Log("新坐标"+newPos);
                if(newPos.x>=tilemap.cellBounds.size.x||newPos.y>=tilemap.cellBounds.size.y)
                {
                    continue;
                }
                if(cameFrom.ContainsKey(newPos))
                {
                    //Debug.Log("已经存在");
                    continue;
                }
                
               /*if(tilemap.GetTile(newPos).name=="Ground")
                {
                    Debug.LogError("是地面");
                    continue;
                }*/
                TileBase tile = tilemap.GetTile(newPos);
                Debug.Log("当前tilemap名称"+tile);
                if (tile != null && tile == borderTile)
                {
                    Debug.Log("是地面");
                    continue;
                }


                //queue.Enqueue(newPos);
                newPositions.Add(newPos);
                cameFrom[newPos] = current;
                //Debug.Log("cameFrom"+cameFrom[newPos]);
            }
            playerPath.AddRange(newPositions);
        }
        Stack<Vector3Int> stackPath = new Stack<Vector3Int>();
        Vector3Int pos=TargetCell;
        while(cameFrom.ContainsKey(pos))
        {
            stackPath.Push(pos);
            pos = cameFrom[pos];
        }
        while(stackPath.Count>0)
        {
            Vector3Int p = stackPath.Pop();
            tilemap.SetTileFlags(p, TileFlags.None);
            //tilemap.SetTile(p, redTile);
            path.Add(p);
           
            
        }
        
       yield return null;
        
    }
    private void ChangeTiles(TileBase tile)
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (var position in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                tilemap.SetTile(position, tile);
            }
        }
    }
  

}
