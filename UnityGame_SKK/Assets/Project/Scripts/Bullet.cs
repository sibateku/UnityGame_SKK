using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action<Vector3> OnBulletHit; // 他クラスに通知するイベント

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.transform.root.gameObject.name == "wall")
        {
            // 衝突した位置をログに出力
            Vector3 hitPoint = other.contacts[0].point;
            Debug.Log("Bullet hit at: " + hitPoint);
            
            // イベント通知
            OnBulletHit?.Invoke(hitPoint);

            // 衝突時に弾を削除
            Destroy(gameObject);
        }
    }
}
