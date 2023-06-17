using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这个的理解难度相比于弹幕追踪要简单很多，实现难度也低很多，所以注释会比较少
public class EnemyMove : MonoBehaviour
{
    public GameObject line;//线路组
    private Transform[] enemyChild;//敌人子物体组
    public EnemyAttribute enemyAttribute;
 
    public List<Transform> line_array;//所有路点位置的数组
    [SerializeField]
    private List<GameObject> activeChildren = new List<GameObject>();
    private Transform current_point;//当前路点位置
    private Transform my_point;//下一个路点位置
 
    [Range(0, 50)]
    public float speed;//移动速度
 
    [Range(0, 2)]
    public int move_method;//移动循环方式
    //0为从出发点开始，到终点时从起点为目标再次开始循环移动
    //1为到了终点后，沿着原路返回，回到起始点后再次按路径到终点
    //2为到了终点后将不再移动
 
    private Rigidbody2D body;//刚体组件
 
    [Range(0, 10)]
    public float point_waittime;//每次到达一个位置后的的等待时间
 
    [Range(0, 10)]
    public float point_distance;//距离路点的位置容忍差
 
    private int index;//标点位置
 
    private bool set = true;//路径移动辅助，默认true
    public bool isFog=false;
    private bool isAttack = false;
    private int beAttack;
    private int maxHealth;
    private int healthThreshold;
    private int activeChildIndex = 0;
 
    void Start()
    {
        
        enemyChild = GetComponentsInChildren<Transform>();
        enemyAttribute = GetComponent<EnemyAttribute>();
        maxHealth = enemyAttribute.enemyData.enemyHP;
        healthThreshold = maxHealth / 7;
        index = 0;
        body = GetComponent<Rigidbody2D>();
        StartCoroutine("DealDamageOverTime");
        line_array = GetChildPositions(line);
        foreach (Transform child in enemyChild)
        {
            if(child.gameObject!=this.gameObject&&child.gameObject.activeSelf)
            {
                activeChildren.Add(child.gameObject);
            }
        }
    }
 
 
    void Update()//根据选择的移动方式进行移动
    {
        if (enemyAttribute.enemyHP <= 0)
        {
            // 当血量小于等于0时关闭当前物体
            gameObject.SetActive(false);
            return;
        }
        int threshold = maxHealth - enemyAttribute.enemyHP;// 计算当前血量对应的阈值
        int childIndex = threshold / healthThreshold;// 计算当前应该激活的子物体的索引
        if (childIndex >= activeChildren.Count)
        {
            childIndex = activeChildren.Count - 1;
        }

        // 按顺序关闭子物体
        for (int i = activeChildIndex+1; i <= childIndex; i++)
        {
            activeChildren[i].SetActive(false);
            activeChildIndex = i;
        }
        Debug.Log(enemyAttribute.enemyHP);
        //Debug.Log(move_method);
        switch (move_method)
        {
            
            case 0:
                Move_line0();
                break;
            case 1:
                Move_line1();
                break;
            case 2:
                Move_line2();
                break;
            case 3:
                Attack();
                break;  
            default:
                break;
        }
    }
 
    private float Get_Distance(Transform point)//获取物体和路点的距离
    {
        return Vector3.Distance(transform.position, point.position);
    }
 
    void Move_line0()//移动方式-0，这个判断格式使得此方法可以复用
    {
        if (Get_Distance(line_array[index]) <= point_distance && set)
        {
            index = (index + 1) % line_array.Count;
        }
        else if (Get_Distance(line_array[index]) <= point_distance && !set)
        {
            index--;
        }
        Move();
    }
 
    void Move_line1()//移动方式-1，到大最后一个点是修改set，使得line0中index从自增改为自减
    {
        if(index == line_array.Count - 1 && set)
        {
            set = false;
        }
        else if(index == 0 && !set)
        {
            set = true;
        }
        Move_line0();
    }
 
    void Move_line2()//判断到达终点后不再进行移动操作
    {
        if(index == line_array.Count -1 && Get_Distance(line_array[index]) <= point_distance)
        {
            return;
        }
        else
        {
            Move_line0();
        }
    }
 
    void Move()//朝向目标移动
    {
        if(isFog)
        {
            Vector2 rot = line_array[index].position - transform.position;
            rot = rot.normalized;
            Vector2 position = transform.position;
            position.x += rot.x * speed * Time.deltaTime;
            position.y += rot.y * speed * Time.deltaTime;
            body.MovePosition(position);
        }

    }
    void Attack()
    {
        if(isAttack)
        {
            //每隔1s enemyAttribute.enemyHp减少beAttack数值

        }
        //停止移动
        this.body.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
     {
        //Debug.Log("触发");
        if(other.gameObject.tag == "Player"){
            beAttack =other.gameObject.GetComponent<PlayerAttribute>().playerATK;
            current_point = line_array[index];
            Debug.Log("当前坐标"+line_array[index].name);
            isAttack = true;
            move_method = 3;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            beAttack =0;
            my_point=this.transform;
            isAttack = false;
            move_method = 2;
        }
    }
    public List<Transform> GetChildPositions(GameObject parent)
    {
        List<Transform> childPositions = new List<Transform>();

        foreach (Transform child in parent.transform)
        {
            childPositions.Add(child);
        }

        return childPositions;
    }
    private IEnumerator DealDamageOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 等待1秒

            if (isAttack)
            {
                enemyAttribute.enemyHP -= beAttack;
            }
        }
    }

}
