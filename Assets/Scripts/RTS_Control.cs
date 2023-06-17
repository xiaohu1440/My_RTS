using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class RTS_Control : MonoBehaviour
{
    [SerializeField]
    private Transform selectionAreaTransform;
    private Vector3 startPositon;
    private List<UnitRTS> selectedUnitList;

    private void Awake()
    {
        selectedUnitList = new List<UnitRTS>();
        selectionAreaTransform.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPositon = UtilsClass.GetMouseWorldPosition();
            selectionAreaTransform.gameObject.SetActive(true);
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(Mathf.Min(startPositon.x, currentMousePosition.x), Mathf.Min(startPositon.y, currentMousePosition.y));
            Vector3 upperRight = new Vector3(Mathf.Max(startPositon.x, currentMousePosition.x), Mathf.Max(startPositon.y, currentMousePosition.y));
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }
        if(Input.GetMouseButtonUp(0))
        {
            selectionAreaTransform.gameObject.SetActive(false);
            Collider2D[] collider2Ds = Physics2D.OverlapAreaAll(startPositon, UtilsClass.GetMouseWorldPosition());
            Debug.Log("#####");
            foreach(UnitRTS unitRTS in selectedUnitList)
            {
                unitRTS.SetSelectedVisible(false);
            }
            selectedUnitList.Clear();
            foreach(Collider2D collider2D in collider2Ds)
            {
                UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();
                if(unitRTS != null)
                {
                    unitRTS.SetSelectedVisible(true);
                    selectedUnitList.Add(unitRTS);
                }


                Debug.Log(collider2D);
            }
            Debug.Log(selectedUnitList.Count);
        }
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 targetPosition = UtilsClass.GetMouseWorldPosition();
            
            foreach(UnitRTS unitRTS in selectedUnitList)
            {
                unitRTS.MoveTo(targetPosition);
            }
        }
    }
}
