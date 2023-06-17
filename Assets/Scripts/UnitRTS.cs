using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRTS : MonoBehaviour
{
    private Transform[] playerChild;
    [SerializeField]
    private Transform[] selectedChild;
    [SerializeField]
    private List<GameObject> activeChildren = new List<GameObject>();
    [SerializeField]
    private List<GameObject> selectedChildren = new List<GameObject>();
    private int maxHealth;
    private int healthThreshold;
    private int activeChildIndex = 0;
    private GameObject selectedGameObject;
    private IMovePosition movePosition;
    private bool isAttack;
    private int beAttack;
    private int enemyNum;
    private bool isDealingDamage = false;
    public PlayerAttribute playerAttribute;
    


    private void Awake()
    {
        playerChild = GetComponentsInChildren<Transform>();
        selectedChild = transform.Find("Click").GetComponentsInChildren<Transform>();
        playerAttribute = GetComponent<PlayerAttribute>();
        maxHealth = playerAttribute.playerData.playerHP;
        healthThreshold = maxHealth / 7;
        selectedGameObject = transform.Find("Click").gameObject;
        movePosition = GetComponent<AsterMove>();
        SetSelectedVisible(false);
        foreach (Transform child in playerChild)
        {
            if(child.gameObject!=this.gameObject&&child.gameObject.activeSelf)
            {
                activeChildren.Add(child.gameObject);
            }
        }
        foreach (Transform child in selectedChild)
        {
            if(child.gameObject!=selectedChild[0].gameObject)
            {
                selectedChildren.Add(child.gameObject);
            }
        }
        
    }

    private void Update() 
    {
     if (playerAttribute.playerHP <= 0)
        {
            // 当血量小于等于0时关闭当前物体
            gameObject.SetActive(false);
            return;
        }
        int threshold = maxHealth - playerAttribute.playerHP;
        int childIndex = threshold / healthThreshold;
        if (childIndex >= activeChildren.Count)
        {
            childIndex = activeChildren.Count-1;
        }
        for (int i = activeChildIndex+1; i <= childIndex; i++)
        {
            activeChildren[i].SetActive(false);
            selectedChildren[i].SetActive(false);
            activeChildIndex = i;
        }
        if(TowerFog.instance.isRevert)
        {
            TowerFog.instance.isRevert = false;
            //activeChildren和selectedChildren所有元素都设置为true
            foreach (GameObject child in activeChildren)
            {
                child.SetActive(true);
            }
            foreach (GameObject child in selectedChildren)
            {
                child.SetActive(true);
            }
        }
    }
    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        movePosition.SetMovePosition(targetPosition, () => { });
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Enemy"&&!isDealingDamage)
        {
            isAttack = true;
            isDealingDamage = true;
            beAttack = other.GetComponent<EnemyAttribute>().enemyATK;
            enemyNum++;
            StartCoroutine(DealDamageOverTime(enemyNum));
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Enemy"&&isDealingDamage)
        {
            isAttack = false;
            isDealingDamage = false;
            enemyNum--;
            beAttack = 0;
            StopCoroutine(DealDamageOverTime(enemyNum));
        }
    }

    private IEnumerator DealDamageOverTime(int enemyNum)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 等待1秒

            if (isAttack)
            {
                playerAttribute.playerHP -= beAttack*enemyNum;
            }
        }
    }

   
}
