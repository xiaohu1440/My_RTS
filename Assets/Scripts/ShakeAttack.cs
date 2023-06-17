using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAttack : MonoBehaviour
{
    public float interval = 1f; // 震动的间隔时间
    public float duration = 0.2f; // 震动的持续时间
    public float strength = 0.1f; // 震动的力度

    private bool isColliding = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            isColliding = true;
            StartCoroutine(Vibrate());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            isColliding = false;
        }
    }

    private IEnumerator Vibrate()
    {
        while (isColliding)
        {
            // 在此处实现震动的逻辑，可以通过修改物体的位置、旋转或缩放来模拟震动效果
            // 以下示例代码通过修改物体的位置来实现震动效果

            Vector3 originalPosition = transform.position;
            Vector3 randomOffset = Random.insideUnitCircle * strength;

            transform.position = originalPosition + randomOffset;

            yield return new WaitForSeconds(duration);

            transform.position = originalPosition;

            yield return new WaitForSeconds(interval);
        }
    }
}
