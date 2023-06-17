using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    private void Awake() {
        mainCamera = Camera.main;

    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scroll * zoomSpeed;

        // 获取当前的相机的位置
        float currentZoom = mainCamera.orthographicSize;

        // 根据滚动调整相机的大小
        float newZoom = currentZoom - zoomAmount;

        // 限制相机的大小在最小值和最大值之间
        newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

        // 更新相机的大小
        mainCamera.orthographicSize = newZoom;
    }
}
